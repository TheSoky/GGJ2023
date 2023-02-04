using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private ScriptableReference _references;

    [SerializeField]
    private ScriptableSaveFile _playerData;

    [SerializeField]
    private float _projectileSpeed = 20.0f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.GetComponent<IDamageable>()?.TakeDamage(_playerData.BaseDamage);
        _references.Pool.ReturnToPool(gameObject, PoolManager.PrefabType.PLAYER_PROJECTILE);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void FireProjectile(Vector2 direction, Vector2 startPos)
    {
        transform.position = startPos;
        _rigidbody.velocity = direction.normalized * _projectileSpeed;
        StartCoroutine(DespawnAtDistance());
    }

    private IEnumerator DespawnAtDistance()
    {
        float timing = _playerData.GetWeaponRange() / _projectileSpeed;
        yield return new WaitForSeconds(timing);
        _references.Pool.ReturnToPool(gameObject, PoolManager.PrefabType.PLAYER_PROJECTILE);
    }
}
