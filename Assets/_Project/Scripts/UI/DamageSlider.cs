using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private TMP_Text _valuesText;

    private ILevelManager _levelManager;

    private void OnDestroy()
    {
        if (_levelManager == null) 
            return;
        
        _levelManager.ScoreChanged -= OnScoreChanged;
        _levelManager.LevelChanged -= OnLevelChanged;
    }

    public void Construct(ILevelManager levelManager)
    {
        _levelManager = levelManager;

        levelManager.ScoreChanged += OnScoreChanged;
        _levelManager.LevelChanged += OnLevelChanged;

        Refresh();
    }

    private void OnScoreChanged(float score, float targetScore)
    {
        _slider.value = _levelManager.GetLevelProgress();

        _valuesText.text = $"{score:0} / {targetScore:0}";
    }

    private void OnLevelChanged(int level)
    { 
        _currentLevelText.text = level.ToString();

        _slider.value = _levelManager.GetLevelProgress();
    }

    private void Refresh()
    {
        _currentLevelText.text = _levelManager.CurrentLevel.ToString();

        _slider.value = _levelManager.GetLevelProgress();
        
        if (_levelManager.IsMaxLevel)
        { 
            _valuesText.text = $"{_levelManager.CurrentScore:0} / MAX";
            return;
        }
        
        _valuesText.text = $"{_levelManager.CurrentScore:0} / {_levelManager.GetNextTargetScore():0}"; 
    }
}