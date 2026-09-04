using System;
using UnityEngine;

public interface IWarpService : IService
{
    event Action<LevelType> TeleportTo;

    void RegisterPlayer(PlayerPresenter player);
    void RegisterSpawnPoint(LevelType type, Transform spawnPoint);
    void RegisterWarp(Warp warp);
    void TeleportingTo(LevelType type);
}