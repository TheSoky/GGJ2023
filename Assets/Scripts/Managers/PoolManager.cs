using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Pool Sub-Classes
    public enum PrefabType : ushort
    {
        PLAYER_PROJECTILE,
        ENEMY_PROJECTILE,
        ENEMY_CHARACTER
    }

    [System.Serializable]
    public class Prefabs : IComparer<PrefabType>, IComparer<GameObject>
    {
        public PrefabType Type;
        public GameObject Prefab;
        public int InitialAmount;

        public int Compare(PrefabType x, PrefabType y)
        {
            return (short)x - (short)y;
        }

        public int Compare(GameObject x, GameObject y)
        {
            return x == y ? 0 : -1;
        }
    }
    #endregion

    [SerializeField]
    private ScriptableReference _poolReference;

    [SerializeField]
    private List<Prefabs> _prefabsToPool = new List<Prefabs>();
    private Dictionary<PrefabType, Queue<GameObject>> _pools = new Dictionary<PrefabType, Queue<GameObject>>();

    private void Awake()
    {
        SetupPools();
        _poolReference.SetPool(this);
    }

    public GameObject RequestPooledItem(PrefabType type)
    {
        if(!_pools.ContainsKey(type))
        {
            Debug.LogError("there is no such type of prefab!");
            return null;
        }
        Queue<GameObject> queue = _pools[type];
        GameObject go;
        if(queue.Count > 0)
        {
            go = queue.Dequeue();
        }
        else
        {
            go = Instantiate(_prefabsToPool.Find(Prefab => Prefab.Type == type).Prefab);
        }
        go.SetActive(true);
        return go;
    }

    public void ReturnToPool(GameObject go, PrefabType type)
    {
        go.SetActive(false);
        if(_pools.ContainsKey(type))
        {
            _pools[type].Enqueue(go);
        }
    }

    public void SetupPools()
    {
        foreach (Prefabs pool in _prefabsToPool)
        {
            if(_pools.ContainsKey(pool.Type))
            {
                Debug.LogError("Just adjust the existing same thing... with bigger number, don't go twice");
                continue;
            }
            if(pool.InitialAmount < 1 || pool.Prefab == null)
            {
                Debug.LogError("Fix Pool data for: " + pool.Type);
                continue;
            }


            Queue<GameObject> items = new Queue<GameObject>();
            for (int i = 0; i < pool.InitialAmount; i++)
            {
                GameObject go = Instantiate(pool.Prefab);
                go.SetActive(false);
                items.Enqueue(go);
            }
            if(items.Count > 0)
            {
                _pools.Add(pool.Type, items);
            }
        }
    }


}
