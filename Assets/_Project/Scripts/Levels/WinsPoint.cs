using TMPro;
using UnityEngine;

public class WinsPoint : MonoBehaviour
{
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private PlayerTriggerObserver _playerTrigger;

    private IWarpService _warpService;

    private void OnDestroy() => 
        _playerTrigger.TriggerEnter -= OnPlayerEnter;

    public void Construct(LevelsStaticData data, IWarpService warpService)
    {
        _warpService = warpService;

        _winsText.text = "+ " + data.Wins.ToString();

        _playerTrigger.TriggerEnter += OnPlayerEnter;
    }

    private void OnPlayerEnter()
    {
        _warpService.TeleportingTo(LevelType.Lobby);
    }
}