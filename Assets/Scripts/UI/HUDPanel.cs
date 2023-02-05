using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDPanel : MonoBehaviour
{
    [SerializeField]
    private ScriptableReference _references;

    [SerializeField]
    private GameObject _hudPanel;

    [SerializeField]
    private GameObject _nextWavePanel;

    [SerializeField]
    private GameObject _pausePanel;

    [SerializeField]
    private GameObject _talentTreePanel;

    [SerializeField]
    private GameObject _nextLevelPanel;

    [SerializeField]
    private TMP_Text _waveCountdownAlertText;

    [SerializeField]
    private string _waveContextText = "New wave in {0}...\nGet ready!";

    [SerializeField]
    private TMP_Text _currentWaveText;

    [SerializeField]
    private string _waveNrString = "Wave: {0}/{1}";

    [SerializeField]
    private TMP_Text _countdownWaveContiniousText;

    [SerializeField]
    private Image _healthBarFill;

    [SerializeField]
    private Image _shieldBarFill;

    private List<WaveData> _levelWaves;
    private int _currentWave;
    private int _totalWaves;
    private float _waveTimer;


    private void Start()
    {
        _references.PlayerController.RegisterHudPanel(this);
        _levelWaves = _references.Spawner.AllWaves;
        _currentWave = 1;
        _totalWaves = _levelWaves.Count;
        _waveTimer = _levelWaves[0].WaveDuration;
        _currentWaveText.text = string.Format(_waveNrString, _currentWave.ToString(), _totalWaves.ToString());
        _references.Spawner._hud = this;
    }

    private void Update()
    {
        _waveTimer -= Time.deltaTime;
        _countdownWaveContiniousText.text = _waveTimer.ToString("00.00");
        _waveCountdownAlertText.text = string.Format(_waveContextText, _waveTimer.ToString("0"));
        if (!_nextWavePanel.activeSelf && _waveTimer <= 5.0f)
        {
            _nextWavePanel.SetActive(true);
        }
        else if(_waveTimer < 0.0f)
        {
            _waveTimer = _levelWaves[_currentWave].WaveDuration;
            _currentWave++;
            _currentWaveText.text = _currentWaveText.ToString();
            _nextWavePanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _references.PlayerController.RegisterHudPanel(null);
    }

    private bool _pauseState = false;

    public void TogglePause()
    {
        _pauseState = !_pauseState;
        _pausePanel.SetActive(_pauseState);
        Time.timeScale = _pauseState ? 0.0f : 1.0f;
    }

    public void OnHealthChanged(float newPercentage)
    {
        _healthBarFill.fillAmount =newPercentage;
    }

    public void OnShieldChanged(float newPercentage)
    {
        _shieldBarFill.fillAmount = newPercentage;
    }

    public void OnNextLevelRequested()
    {
        _hudPanel.SetActive(false);
        _nextLevelPanel.SetActive(true);
    }

}
