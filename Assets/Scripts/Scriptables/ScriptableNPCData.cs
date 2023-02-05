using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData00", menuName = "GJJ/New NPC")]
public class ScriptableNPCData : ScriptableObject
{

    [Header("Basic stats")]
    public int InitalHealth;
    public float AttackRange;
    public float AttackRate;
    public float AttackDamage;
    public float MovementSpeed;
    public AttackType AttackType;
    public float DetectionUpdateTime;

    [Header("Visuals")]
    public Animations Attack;
    public Animations Idle;
    public Animations Walk;

}

public enum AttackType : ushort
{
    MELEE,
    RANGED,
    KAMIKAZE
}