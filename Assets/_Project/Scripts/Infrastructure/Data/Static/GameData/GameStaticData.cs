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
    [Header("Weapons")]
    public WeaponViewsData WeaponsView;
    public WeaponStaticData[] WeaponsStaticData;
}
