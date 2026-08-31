using System;
using UnityEngine;

[CreateAssetMenu(fileName ="GameStaticData", menuName = "StaticData/GameData")]
public class GameStaticData : ScriptableObject
{
    public Vector3 LobbySpawnPosition;

    public CameraSettingsData CameraSettingsData;

    [Space]
    [Header("Weapon Settings")]
    [Tooltip("Маски коллизии для оружия")]
    public LayerMask AimLayerMask;

    [Space]
    [Header("Levels")]
    public LevelView[] LevelViews;
    public LevelsStaticData[] LevelsData;

    [Space]
    [Header("Obstacles")]
    public ObstacleStaticData[] Obstacles;

    [Space]
    [Header("Enemies")]
    public EnemyStaticData[] EnemiesStaticData;

    [Space]
    [Header("SpawnPointsData")]
    public SpawnPointsData SpawnPoints;

    [Space]
    [Header("Weapons")]
    public WeaponViewsData WeaponsView;
    public WeaponStaticData[] WeaponsStaticData;
}

[Serializable]
public class SpawnPointsData
{
    public ObstaclesPointsData Obstacles;
    public EnemiesSpawnData Enemies;
}

[Serializable]
public class EnemiesSpawnData
{
    public EnemySpawnPointData[] EnemiesSpawnPointData;
}

[Serializable]
public class EnemySpawnPointData
{
    public EnemyType Type;
    public Vector3 Position;

    public EnemySpawnPointData(EnemyType type, Vector3 position)
    {
        Type = type;
        Position = position;
    }
}

[Serializable]
public class SoldierSpawnPointData : EnemySpawnPointData
{
    public SoldierAttackPointData AttackPointData;

    public SoldierSpawnPointData(EnemyType type, Vector3 position, SoldierAttackPointData attackPoint) : base(type, position)
    {
        AttackPointData = attackPoint;
    }
}

[Serializable]
public class SoldierAttackPointData
{
    public Vector3 Position;

    public SoldierAttackPointData(Vector3 position)
    {
        Position = position;
    }
}

[Serializable]
public class ObstaclesPointsData
{
    public ObstacleSpawnPointData[] ObstaclesSpawnData;
}

[Serializable]
public class ObstacleSpawnPointData
{
    public ObstacleType Type;
    public Vector3 Position;

    public ObstacleSpawnPointData(ObstacleType type, Vector3 position)
    {
        Type = type;
        Position = position;
    }
}