using TMPro;
using UnityEngine;

public class WinsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _winsText;

    private ILevelManager _levelManager;

    private void OnDestroy() => 
        _levelManager.WinsChanged -= UpdateText;

    public void Construct(ILevelManager levelManager)
    {
        _levelManager = levelManager;

        _levelManager.WinsChanged += UpdateText;

        UpdateText(_levelManager.Wins);
    }

    private void UpdateText(int value) => 
        _winsText.text = value.ToString();
}