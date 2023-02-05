using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ScriptableReference _references;

    [SerializeField]
    private Animator _frontPart;

    [SerializeField]
    private Animator _backPart;

    [SerializeField]
    private Collider2D _playerDetecionCollider;

    [SerializeField]
    private Collider2D _meleeAttackCollider;

    [SerializeField]
    private Collider2D _explosionCollider;

    [SerializeField]
    private Transform _firePosition;

    [SerializeField]
    private LayerMask _hittableMeleeLayers;

    private float _health;
    private float _timer = 0.0f;
    private ScriptableNPCData _current;
    private NavMeshAgent _agent;
    private ContactFilter2D _filter;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _filter = new ContactFilter2D()
        {
            layerMask = _hittableMeleeLayers,
            useLayerMask = true,
        };
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _current.DetectionUpdateTime)
        {
            if(_playerDetecionCollider.IsTouchingLayers())
            {
                _agent.ResetPath();
                StartCoroutine(AttackingCoroutine());
            }
            else
            {
                _agent.SetDestination(_references.PlayerTransform.position);
                SetAnimation(_current.Walk);
            }
            _timer = 0.0f;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetupNpc(ScriptableNPCData npc)
    {
        _timer = 0.0f;
        _current = npc;
        _health = npc.InitalHealth;
        _agent.speed = _current.MovementSpeed;
        StartCoroutine(WaitForNavReady());
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if(_health <= 0.0f)
        {
            //TODO drop resource
            _references.Pool.ReturnToPool(gameObject, PoolManager.PrefabType.ENEMY_CHARACTER);
        }
    }

    private void SetAnimation(Animations anims)
    {
        _frontPart.Play(anims.FrontAnimation.name);
        _backPart.Play(anims.BackAnimation.name);
    }

    private IEnumerator AttackingCoroutine()
    {

        float timeBetweenAttacks = 1 / _current.AttackRate;
        float _timer = _current.DetectionUpdateTime;
        while(_timer > 0)
        {
            transform.up = (_references.PlayerTransform.position - transform.position).normalized;

            StartCoroutine(AttackCoroutine());

            yield return new WaitForSeconds(timeBetweenAttacks);
            _timer -= timeBetweenAttacks;
        }
        SetAnimation(_current.Walk);
    }

    private IEnumerator AttackCoroutine()
    {
        SetAnimation(_current.Attack);
        float midTime = _current.Attack.FrontAnimation.clip.length / 2;
        yield return new WaitForSeconds(midTime);
        if(_current.AttackType == AttackType.RANGED)
        {
            GameObject projectile = _references.Pool.RequestPooledItem(PoolManager.PrefabType.ENEMY_PROJECTILE);
            projectile.GetComponent<ProjectileBehaviour>().FireProjectile(transform.up, _firePosition.position, _current.AttackRange, _current.AttackDamage);
        }
        else if (_current.AttackType == AttackType.MELEE)
        {
            Collider2D[] results = new Collider2D[20];
            int amount = _meleeAttackCollider.OverlapCollider(_filter, results);
            amount = Mathf.Min(amount, 20);
            for (int i = 0; i < amount; i++)
            {
                results[i].GetComponent<IDamageable>()?.TakeDamage(_current.AttackDamage);
            }
        }
        else
        {
            Collider2D[] results = new Collider2D[20];
            int amount = _explosionCollider.OverlapCollider(_filter, results);
            amount = Mathf.Min(amount, 20);
            for (int i = 0; i < amount; i++)
            {
                results[i].GetComponent<IDamageable>()?.TakeDamage(_current.AttackDamage);
            }
        }
        yield return new WaitForSeconds(midTime);
        SetAnimation(_current.Idle);
    }

    private IEnumerator WaitForNavReady()
    {
        yield return new WaitUntil(() => _agent.isOnNavMesh);
        _agent.SetDestination(_references.PlayerTransform.position);
        SetAnimation(_current.Walk);
    }

}

[System.Serializable]
public class Animations
{
    public Animation FrontAnimation;
    public Animation BackAnimation;
}