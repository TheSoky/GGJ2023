using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ScriptableSaveFile _playerSave;

    [SerializeField]
    private Transform _frontBody;

    private Rigidbody2D _rigidbody;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Vector2 _aimDirection;

    private void Awake()
    {
        if(_playerSave == null)
        {
            Debug.LogError("Missing Save file on player! what are you trying to pull of?");
            Destroy(this);
        }
        _playerSave.LoadData();
        _rigidbody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        if(context.canceled)
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
        if(context.canceled)
        {
            return;
        }
        Vector3 target;
        if(_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            target = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
            target.z = 0.0f;
        }
        else
        {
            target = (Vector3)context.ReadValue<Vector2>() + _frontBody.position;
        }
        _frontBody.rotation = Quaternion.FromToRotation(_frontBody.position, target);
    }

    private void OnDestroy()
    {
        if(_playerSave != null)
        {
            _playerSave.SaveData();
        }
    }
}
