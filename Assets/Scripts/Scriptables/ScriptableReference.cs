using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableReference", menuName = "GJJ/Scriptable Reference File")]
public class ScriptableReference : ScriptableObject
{
    private PoolManager _poolManager;
    public PoolManager Pool { get => _poolManager; }

    public void SetPool(PoolManager pool)
    {
        _poolManager = pool;
    }

    public void ClearPool()
    {
        _poolManager = null;
    }
}
