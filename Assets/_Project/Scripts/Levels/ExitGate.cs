using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ExitGate : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private LocalizedString _levelLocale;
    [SerializeField] private TMP_Text _recommendedLevel;
    [SerializeField] private Image[] _emojis;

    private LevelsStaticData _data;

    private void OnDestroy()
    {
        if (_levelText != null)
            _levelLocale.StringChanged -= UpdateLevelText;
    }

    public void Construct(LevelsStaticData data)
    {
        _data = data;

        if (_levelText != null)
        {            
            UpdateLevelText(_levelLocale.GetLocalizedString());
            _levelLocale.StringChanged += UpdateLevelText;
        }

        if (_recommendedLevel != null)
            _recommendedLevel.text = data.Recommended.ToString();

        if (data.Icon != null)
            foreach (var icon in _emojis)
                icon.sprite = data.Icon;
    }

    private void UpdateLevelText(string value) => 
        _levelText.text = string.Format(value, ((int)_data.Type).ToString());
}
