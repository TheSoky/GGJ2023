using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableReference", menuName = "GJJ/Scriptable Reference File")]
public class ScriptableReference : ScriptableObject
{
    private PoolManager _poolManager;
    public PoolManager Pool { get => _poolManager; }

    private PlayerController _playerController;
    public PlayerController PlayerController { get => _playerController; }

    [SerializeField] private ScriptableSaveFile _playerData;
    public ScriptableSaveFile PlayerData { get => _playerData; }

    private NPCSpawnManager _spawner;
    public NPCSpawnManager Spawner { get => _spawner; }

    public void SetSpawner(NPCSpawnManager spawner)
    {
        _spawner = spawner;
    }

    public void SetPool(PoolManager pool)
    {
        _poolManager = pool;
    }

    public void ClearPool()
    {
        _poolManager = null;
    }

    public void SetPlayer(PlayerController player)
    {
        _playerController = player;
    }

    public void ClearPlayer()
    {
        _playerController = null;
    }
}
