using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExitGate : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _recommendedLevel;
    [SerializeField] private Image[] _emojis;

    public void Construct(LevelsStaticData data)
    {
        if (_levelText != null)
            _levelText.text = ((int)data.Type).ToString();

        if (_recommendedLevel != null)
            _recommendedLevel.text = data.Recommended.ToString();

        if (data.Icon != null)
            foreach (var icon in _emojis)
                icon.sprite = data.Icon;
    }
}
