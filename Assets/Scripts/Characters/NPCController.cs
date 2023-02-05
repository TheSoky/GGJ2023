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
    private CircleCollider2D _playerDetecionCollider;

    [SerializeField]
    private Collider2D _meleeAttackCollider;

    [SerializeField]
    private Collider2D _explosionCollider;

    [SerializeField]
    private Transform _firePosition;

    [SerializeField]
    private LayerMask _hittableMeleeLayers;

    
    private SpriteRenderer _cloudSprite;
    private float _health;
    private float _timer = 0.0f;
    private ScriptableNPCData _current;
    private NavMeshAgent _agent;
    private ContactFilter2D _filter;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
        _cloudSprite = _explosionCollider.GetComponent<SpriteRenderer>();
        _filter = new ContactFilter2D()
        {
            layerMask = _hittableMeleeLayers,
            useLayerMask = true,
        };
    }

    private void OnEnable()
    {
        _cloudSprite.color = Color.clear;
    }

    private void Update()
    {
        if (_current != null)
        {
            _timer += Time.deltaTime;

            if (_timer > _current.DetectionUpdateTime)
            {
                transform.up = (_references.PlayerTransform.position - transform.position).normalized;
                if (_playerDetecionCollider.IsTouchingLayers())
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
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetupNpc(ScriptableNPCData npc, Vector3 position)
    {
        position.z += 0.4f;
        transform.position = position;
        transform.rotation = Quaternion.identity;
        _timer = 0.0f;
        _current = npc;
        _health = npc.InitalHealth;
        _agent.speed = _current.MovementSpeed;
        _playerDetecionCollider.radius = _current.AttackRange;
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
        float midTime = _current.Attack.FrontAnimation.length / 2;
        if(_current.AttackType == AttackType.KAMIKAZE)
        {
            StartCoroutine(SmokeCloudEffect(midTime));
        }
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
            _references.Pool.ReturnToPool(gameObject, PoolManager.PrefabType.ENEMY_CHARACTER);
        }
        yield return new WaitForSeconds(midTime);
        SetAnimation(_current.Idle);
    }

    private IEnumerator SmokeCloudEffect(float midTime)
    {
        float timer = 0.0f;
        while(timer < midTime)
        {
            _cloudSprite.color = Color.Lerp(Color.clear, Color.white, timer / midTime);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer > 0.0f)
        {
            _cloudSprite.color = Color.Lerp(Color.clear, Color.white, timer / midTime);
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WaitForNavReady()
    {
        yield return new WaitUntil(() => _agent.isOnNavMesh);
        transform.up = (_references.PlayerTransform.position - transform.position).normalized;
        _agent.SetDestination(_references.PlayerTransform.position);
        SetAnimation(_current.Walk);
    }

}

[System.Serializable]
public class Animations
{
    public AnimationClip FrontAnimation;
    public AnimationClip BackAnimation;
}