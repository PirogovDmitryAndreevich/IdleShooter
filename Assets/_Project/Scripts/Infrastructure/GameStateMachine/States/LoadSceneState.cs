using System;
using UnityEngine;

public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly IWeaponDataService _weaponDataService;
    private readonly IStaticDataService _staticData;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IGameFactory gameFactory, IWeaponDataService weaponDataService, IStaticDataService staticData)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
        _weaponDataService = weaponDataService;
        _staticData = staticData;
    }

    public void Enter(string payload)
    {
        _sceneLoader.Load(payload, OnLoaded);
    }

    public void Exit()
    {
        
    }

    private void OnLoaded()
    {
        GameStaticData gameData = _staticData.GetGameData();

        Lobby lobby = InitLobby();

        Transition transition1 = InitTransition(lobby, lobby.LevelType);

        LevelView_1 level_1 = InitLevel1(transition1);
        _weaponDataService.ForInitialize();
    private Lobby InitLobby()
    {
        Lobby lobby = _gameFactory.CreateLobby();

        _gameFactory.CreatePlayer(lobby.PlayerSpawnPoint.transform.position);

        foreach (WeaponSwitcherSpawnPoint switcher in lobby.WeaponSwitcherSpawnPoint)
            _gameFactory.CreateWeaponSwitcher(switcher.WeaponType, switcher.transform.position);

        return lobby;
    }

    private Transition InitTransition(LevelView levelView, LevelType type)
    {
        Transition transition =
            _gameFactory.CreateTransition(levelView.ExitPoint);

        return transition;
    }

    private LevelView_1 InitLevel1(Transition transition)
    {
        LevelView_1 level = CreateLevel<LevelView_1>(transition, LevelType.Level_1);

        foreach (var targetPoint in level.TargetsSpawnPoints)
            _gameFactory.CreateTarget(targetPoint.transform.position);

        return level;
    }


    private T CreateLevel<T>(Transition transition, LevelType type) where T : LevelView => 
        _gameFactory.CreateLevel<T>(transition.ExitPoint, type);
}