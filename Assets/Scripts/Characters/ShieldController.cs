using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ScriptableSaveFile _saveFile;

    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private bool _regenerating;

    private HUDPanel _hudPanel;
    private float _regenRate;
    private float _modifier;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _regenerating = _saveFile.Save.DefenseLevel > 0 && !_saveFile.Save.IsOffenseChosen;
        _regenRate = _saveFile.BaseShield / 15;
        _modifier = _saveFile.Save.DefenseLevel > 1 && !_saveFile.Save.IsOffenseChosen ?
            1.5f : 1.0f;

    }

    private void Update()
    {
        if(_regenerating && _saveFile.Save.ShieldRemaining < _saveFile.BaseShield)
        {
            TakeDamage(-_regenRate * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        _saveFile.Save.ShieldRemaining -= amount * _modifier;
        _hudPanel.OnShieldChanged();
        if (_saveFile.Save.ShieldRemaining <= 0.0f)
        {
            _renderer.enabled = false;
            _collider.enabled = false;
            _regenerating = false;
        }
    }

    public void RegisterHudPanel(HUDPanel panel)
    {
        _hudPanel = panel;
    }

}
