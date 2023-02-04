using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData00", menuName = "GJJ/New NPC")]
public class ScriptableNPCData : ScriptableObject
{

    [Header("Basic stats")]
    public int InitalHealth;
    public float AttackRange;
    public float MovementSpeed;
    public bool IsSuicidal;
    public AttackType Attack;
    public CharacterVisuals Visuals;
}

public enum AttackType : ushort
{
    MELEE,
    RANGED,
    KAMIKAZE
}

[System.Serializable]
public class CharacterVisuals
{
    public Animation Stand;
    public Animation Walk;
    public Animation Attack;
}