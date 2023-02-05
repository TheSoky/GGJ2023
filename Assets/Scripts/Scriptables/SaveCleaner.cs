using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCleaner : MonoBehaviour
{
    public ScriptableSaveFile SaveFile;

    private void Start()
    {
        SaveFile.OffenseLevel = 0;
        SaveFile.DefenseLevel = 0;
    }
}
