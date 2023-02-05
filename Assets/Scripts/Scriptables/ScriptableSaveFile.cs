using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "OneAndOnly", menuName = "GJJ/TaknesLiBitCeGuzvus")]
public class ScriptableSaveFile : ScriptableObject
{
    [Header("tuning")]
    public float MaxHealth;
    public float BaseShield;
    public float BaseWeaponRange;
    public float BaseFireRate;
    public float BaseMovementSpeed;
    public float BaseDamage;

    public bool IsOffenseChosen;
    public int CollectedResource;
    public int OffenseLevel;
    public int DefenseLevel;
    public float HealthRemaining;
    public float ShieldRemaining;

    [Header("Only for eyes")]

    private const string FILE_PATH = "savegame.json";

    public float GetWeaponRange()
    {
        return OffenseLevel > 0 && IsOffenseChosen ? BaseWeaponRange : BaseWeaponRange * 2;
    }

    public void ResetData()
    {

        HealthRemaining = MaxHealth;
        ShieldRemaining = BaseShield;
    }
}