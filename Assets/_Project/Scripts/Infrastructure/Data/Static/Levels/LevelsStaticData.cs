using System;
using UnityEngine;

[Serializable]
public class LevelsStaticData
{
    public LevelType Type;

    [Min(1)]
    public int Recommended;

    [Min(0)]
    public int Wins;

    public Sprite Icon;

    public EnemyType[] EnemiesOnLevel;
    public ObstacleType[] ObstaclesOnLevel;
}