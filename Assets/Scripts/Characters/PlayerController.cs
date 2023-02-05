using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ScriptableReference _references;

    [SerializeField]
    private ScriptableSaveFile _playerSave;

    [SerializeField]
    private Transform _frontBody;

    [SerializeField]
    private Transform _firePoint;

    [SerializeField]
    private GameObject _shield;

    private Rigidbody2D _rigidbody;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Vector2 _aimDirection;
    private Coroutine _shootingCoroutine;
    private bool _isOnCooldown = false;

    private void Awake()
    {
        if (_playerSave == null)
        {
            Debug.LogError("Missing Save file on player! what are you trying to pull of?");
            Destroy(this);
        }
        _references.SetPlayer(transform);
        _playerSave.LoadData();
        _rigidbody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
        _shield.SetActive(false);
    }

    private void OnDestroy()
    {
        _references.ClearPlayer();
        if (_playerSave != null)
        {
            _playerSave.SaveData();
        }
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        Vector2 direction = context.ReadValue<Vector2>();
        _rigidbody.velocity = direction * _playerSave.BaseMovementSpeed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    public void OnPlayerAim(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            return;
        }
        
        if (_playerInput.currentControlScheme == "Keyboard&Mouse") //Adapt Mouse position from world to relevant local
        {
            _aimDirection = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>()); //read the world position
            _aimDirection = (_aimDirection - (Vector2)_frontBody.position).normalized; // Normalize as local difference
        }
        else
        {
            _aimDirection = context.ReadValue<Vector2>();
        }
        _frontBody.up = _aimDirection;
    }

    public void OnPlayerShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _shootingCoroutine = StartCoroutine(ShootingCoroutine());
        }
        else if (context.canceled)
        {
            StopCoroutine(_shootingCoroutine);
            StopAllCoroutines(); //HACK, ugly, gamepad is messing around!
        }
    }

    public void OnPlayerShield(InputAction.CallbackContext context)
    {
        if(context.started || context.canceled)
        {
            _shield.SetActive(context.started);
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        while(true)
        {
            FireProjectile();
            yield return new WaitForSeconds(_playerSave.BaseFireRate);
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = _references.Pool.RequestPooledItem(PoolManager.PrefabType.PLAYER_PROJECTILE);
        projectile.GetComponent<ProjectileBehaviour>().FireProjectile(_aimDirection, _firePoint.position, _playerSave.BaseWeaponRange, _playerSave.BaseDamage);
    }

    public void TakeDamage(float amount)
    {
        _playerSave.Save.HealthRemaining -= amount;
        if(_playerSave.Save.HealthRemaining <= Mathf.Epsilon)
        {
            //TODO reload dat level
            _playerSave.Save.HealthRemaining = _playerSave.MaxHealth;
        }
    }
}
