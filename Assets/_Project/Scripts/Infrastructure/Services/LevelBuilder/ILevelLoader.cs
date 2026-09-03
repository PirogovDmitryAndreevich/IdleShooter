using System;
using UnityEngine;

public interface ILevelLoader : IService
{
    LevelLoadOperation InitLevel(LevelType type, Transform position);
}