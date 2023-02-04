using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ScriptableSaveFile _saveFile;

    public void TakeDamage(float amount)
    {
        _saveFile.Save.ShieldRemaining -= amount;
    }

}
