using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public int Amount;
    public Transform position;
    public ScriptableNPCData NPC;
}

[System.Serializable]
public class WaveData
{
    public List<SpawnData> WaveSpawns;
    public int WaveDuration;
}

public class NPCSpawnManager : MonoBehaviour
{
    [SerializeField]
    private ScriptableReference _references;

    [SerializeField]
    private List<WaveData> _allWaves = new List<WaveData>();

    public List<WaveData> AllWaves { get => _allWaves; }

    private int _numberOfMinions = 0;

    public HUDPanel _hud;

    private void Awake()
    {
        _references.SetSpawner(this);
        for (int i = 0; i < AllWaves.Count; i++)
        {
            for (int j = 0; j < AllWaves[i].WaveSpawns.Count; j++)
            {
                _numberOfMinions += AllWaves[i].WaveSpawns[j].Amount;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(LevelWaveLoop());
    }

    private void OnDestroy()
    {
        _references.SetSpawner(null);
        StopAllCoroutines();
    }

    private IEnumerator LevelWaveLoop()
    {
        foreach (WaveData wave in _allWaves)
        {
            foreach (SpawnData spawn in wave.WaveSpawns)
            {
                Vector2 offSet = Vector3.right;
                for (int i = 0; i < spawn.Amount; i++)
                {
                    GameObject npc = _references.Pool.RequestPooledItem(PoolManager.PrefabType.ENEMY_CHARACTER);
                    NPCController npcController = npc.GetComponent<NPCController>();
                    npcController.SetupNpc(spawn.NPC, spawn.position.position + (Vector3)offSet * i);
                }
            }
            yield return new WaitForSeconds(wave.WaveDuration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "crossed-swords.png", false, Color.red);
    }

    public void KillMinion()
    {
        _numberOfMinions--;
        if (_numberOfMinions < 1)
        {
            _hud.OnNextLevelRequested();
        }
    }
}
