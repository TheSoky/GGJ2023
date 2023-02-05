using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableReference", menuName = "GJJ/Scriptable Reference File")]
public class ScriptableReference : ScriptableObject
{
    private PoolManager _poolManager;
    public PoolManager Pool { get => _poolManager; }

    private Transform _playerTransform;
    public Transform PlayerTransform { get => _playerTransform; }

    public void SetPool(PoolManager pool)
    {
        _poolManager = pool;
    }

    public void ClearPool()
    {
        _poolManager = null;
    }

    public void SetPlayer(Transform player)
    {
        _playerTransform = player;
    }

    public void ClearPlayer()
    {
        _playerTransform = null;
    }
}
