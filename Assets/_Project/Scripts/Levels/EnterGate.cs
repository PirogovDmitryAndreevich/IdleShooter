using TMPro;
using UnityEngine;

public class EnterGate : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _recommendedLevel;

    public void Construct(LevelsStaticData data)
    {
        _levelText.text = ((int)data.Type).ToString();

        _recommendedLevel.text = data.Recommended.ToString();
    }
}