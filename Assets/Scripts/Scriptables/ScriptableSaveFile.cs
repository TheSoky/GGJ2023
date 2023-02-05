using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "OneAndOnly", menuName = "GJJ/TaknesLiBitCeGuzvus")]
public class ScriptableSaveFile : ScriptableObject
{
    [Header("tuning")]
    public int MaxHealth;
    public int BaseShield;
    public float BaseWeaponRange;
    public float BaseFireRate;
    public float BaseMovementSpeed;
    public float BaseDamage;

    [Header("Only for eyes")]
    public PermaData Save;

    private const string FILE_PATH = "savegame.json";

    public float GetWeaponRange()
    {
        return Save.OffenseLevel > 0 ? BaseWeaponRange : BaseWeaponRange * 2;
    }

    public void SaveData()
    {
        if(Save != null)
        {
            string json = JsonUtility.ToJson(Save);
            string path = Path.Combine(Application.persistentDataPath, FILE_PATH);
            File.WriteAllText(path, json);
        }
    }

    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, FILE_PATH);
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Save = JsonUtility.FromJson<PermaData>(json);
        }
        else
        {
            Save = new PermaData()
            {
                HealthRemaining = MaxHealth,
                ShieldRemaining = BaseShield
            };
        }
    }

    [Serializable]
    public class PermaData
    {
        public bool IsOffenseChosen;
        public int CollectedResource;
        public int OffenseLevel;
        public int DefenseLevel;
        public int LevelReached;
        public float HealthRemaining;
        public float ShieldRemaining;
    }
}