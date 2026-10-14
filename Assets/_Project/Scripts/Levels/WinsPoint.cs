using System;
using TMPro;
using UnityEngine;

public class WinsPoint : MonoBehaviour
{
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private PlayerTriggerObserver _playerTrigger;

    private ILevelController _levelController;
    private ILevelManager _levelManager;
    private int _winsCount;

    public event Action PlayerGiveWins;

    private void OnDestroy() => 
        _playerTrigger.TriggerEnter -= OnPlayerEnter;

    public void Construct(LevelsStaticData data, ILevelController levelController, ILevelManager levelManager)
    {
        _levelController = levelController;
        _levelManager = levelManager;

        _winsCount = data.Wins;
        _winsText.text = "+ " +_winsCount.ToString();

        _playerTrigger.TriggerEnter += OnPlayerEnter;
    }

    private void OnPlayerEnter()
    {
        _levelController.WarpToLevel(LevelType.Lobby);
        _levelManager.AddWins(_winsCount);
        PlayerGiveWins?.Invoke();
    }
}