using System;

public interface ILevelController : IService, IDisposable
{
    void SwitchLevel(LevelType type);
    void WarpToLevel(LevelType type);
}