using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ScriptableSaveFile _saveFile;

    private HUDPanel _hudPanel;

    public void TakeDamage(float amount)
    {
        _saveFile.Save.ShieldRemaining -= amount;
        _hudPanel.OnShieldChanged(_saveFile.Save.ShieldRemaining / _saveFile.BaseShield);
    }

    public void RegisterHudPanel(HUDPanel panel)
    {
        if(_hudPanel != null)
        {
            _hudPanel = panel;
        }
    }

}
