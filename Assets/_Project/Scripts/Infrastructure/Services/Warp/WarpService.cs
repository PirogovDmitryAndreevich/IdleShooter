using System;
using System.Collections.Generic;
using UnityEngine;

public class WarpService : IDisposable, IWarpService
{
    private readonly Dictionary<LevelType, Transform> _spawnPoints = new();
    private readonly List<Warp> _warps = new();

    private PlayerPresenter _player;

    public event Action<LevelType> TeleportTo;

    public void RegisterPlayer(PlayerPresenter player) =>
        _player = player;

    public void RegisterWarp(Warp warp)
    {
        _warps.Add(warp);

        warp.WarpToLevel += OnWarp;
    }

    public void RegisterSpawnPoint(LevelType type, Transform spawnPoint)
    {
        _spawnPoints[type] = spawnPoint;
    }

    public void TeleportingTo(LevelType type)
    {
        if (!_spawnPoints.TryGetValue(
                type,
                out Transform spawnPoint)) 
            return;        

        _player.Teleport(spawnPoint);
    }

    private void OnWarp(LevelType type) =>
        TeleportTo?.Invoke(type);

    public void Dispose()
    {
        if (_warps != null)
        {
            foreach (Warp warp in _warps)
                warp.WarpToLevel -= OnWarp;
        }

        _player = null;

        _warps.Clear();
        _spawnPoints.Clear();

        TeleportTo = null;
    }
}