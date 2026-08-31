using System;
using System.Collections.Generic;

public class WarpStarter : IDisposable
{
    private readonly Dictionary<LevelType, Transition> _spawnPoints;

    private List<Warp> _warps;
    private PlayerPresenter _player;

    public WarpStarter()
    {
        _spawnPoints = new Dictionary<LevelType, Transition>();
    }

    public void Initialize(List<Warp> warps, PlayerPresenter player)
    {
        _warps = warps;
        _player = player;

        foreach (var warp in _warps)
            warp.WarpToLevel += OnWarp;
    }

    public void Dispose()
    {
        foreach (var warp in _warps)
            warp.WarpToLevel -= OnWarp;
    }

    public void RegisterTransition(Transition transition)
    {
        _spawnPoints.Add(transition.Type, transition);
    }

    private void OnWarp(LevelType type)
    {
        if (_spawnPoints.TryGetValue(type, out Transition transition))
            _player.Teleport(transition.PlayerSpawnPoint);
    }
}