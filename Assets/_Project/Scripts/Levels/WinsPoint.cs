using TMPro;
using UnityEngine;

public class WinsPoint : MonoBehaviour
{
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private PlayerTriggerObserver _playerTrigger;

    private ILevelController _levelController;

    private void OnDestroy() => 
        _playerTrigger.TriggerEnter -= OnPlayerEnter;

    public void Construct(LevelsStaticData data, ILevelController levelController)
    {
        _levelController = levelController;

        _winsText.text = "+ " + data.Wins.ToString();

        _playerTrigger.TriggerEnter += OnPlayerEnter;
    }

    private void OnPlayerEnter()
    {
        _levelController.WarpToLevel(LevelType.Lobby);
    }
}