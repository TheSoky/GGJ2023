using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float _actualWeaponDamage;
    private float _actualFireRate;
    private bool _hasThatExtraLife;

    private HUDPanel _hudPanel;

    private void Awake()
    {
        if (_playerSave == null)
        {
            Debug.LogError("Missing Save file on player! what are you trying to pull of?");
            Destroy(this);
        }
        _references.SetPlayer(this);
        if(_playerSave.Save == null)
        {
            _playerSave.Save = new();
        }
        _playerSave.Save.HealthRemaining = _playerSave.MaxHealth;
        _playerSave.Save.ShieldRemaining = _playerSave.BaseShield;
        _rigidbody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
        _shield.SetActive(false);
        _actualWeaponDamage = (_playerSave.Save.OffenseLevel > 1 && _playerSave.Save.IsOffenseChosen) ?
            _playerSave.BaseDamage * 1.5f : _playerSave.BaseDamage;
        _actualFireRate = _playerSave.Save.OffenseLevel > 2 && _playerSave.Save.IsOffenseChosen ?
            _playerSave.BaseDamage / 2.0f : _playerSave.BaseDamage;
        _hasThatExtraLife = _playerSave.Save.DefenseLevel > 2 && !_playerSave.Save.IsOffenseChosen;
    }

    private void OnDestroy()
    {
        _references.ClearPlayer();
        if (_playerSave != null)
        {
            _playerSave.SaveData();
        }
    }

    public void RegisterHudPanel(HUDPanel panel)
    {
        _hudPanel = panel;
        _shield.GetComponent<ShieldController>().RegisterHudPanel(panel);
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

    public void OnPauseRequested(InputAction.CallbackContext context)
    {
        if (context.started)
            _hudPanel.TogglePause();
    }

    public void CheatingRequested(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _references.Spawner.KillMinion();
        }
    }


    private IEnumerator ShootingCoroutine()
    {
        while(true)
        {
            FireProjectile();
            yield return new WaitForSeconds(_actualFireRate);
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = _references.Pool.RequestPooledItem(PoolManager.PrefabType.PLAYER_PROJECTILE);
        projectile.GetComponent<ProjectileBehaviour>().FireProjectile(_aimDirection, _firePoint.position, _playerSave.GetWeaponRange(), _actualWeaponDamage);
    }

    public void TakeDamage(float amount)
    {
        _playerSave.Save.HealthRemaining -= amount;

        _hudPanel.OnHealthChanged();

        if(_playerSave.Save.HealthRemaining <= 0.0f)
        {
            if(_hasThatExtraLife)
            {
                _hasThatExtraLife = false;
                _playerSave.Save.HealthRemaining = _playerSave.MaxHealth;
                _hudPanel.OnHealthChanged();
            }
            else
            {
                _playerSave.ResetData();
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

}
