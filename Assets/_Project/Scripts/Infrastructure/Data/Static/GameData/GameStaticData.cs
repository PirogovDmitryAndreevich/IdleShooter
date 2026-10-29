using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStaticData", menuName = "StaticData/GameData")]
public class GameStaticData : ScriptableObject
{
    public Vector3 LobbySpawnPosition;
    public AudioClip BackgroundMusic;
    public int ReviewScore = 100;

    [Space]
    [Header("Camera Settings")]
    public CameraSettingsData CameraSettingsData;

    [Space]
    [Header("Weapon Settings")]
    [Tooltip("Маски коллизии для оружия")]
    public LayerMask AimLayerMask;

    [Space]
    [Header("Player")]
    public int MaxLevel;
    public PlayerLevelsStaticData[] PlayerLevelsData;

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
    [Header("Weapons")]
    public WeaponViewsData WeaponsView;
    public WeaponStaticData[] WeaponsStaticData;

    [Space]
    [Header("ShootUIEffectAnimation settings")]
    public ShootUIEffectAnimationSettings ShootUIEffectAnimationSettings;

    [Space]
    [Header("Sounds Effects")]
    public SoundsEffects SoundsEffects;

    [Space]
    [Header("Gifts")]
    public GiftsData[] Gifts;

    [Space]
    [Header("Sounds")]
    public SoundsData Sounds;
}

[Serializable]
public class SoundsData
{
    [Header("UI Sounds")]
    public AudioClip LevelUp;
    public AudioClip OpenWindow;
    public AudioClip CloseWindow;
    public AudioClip Slider;
    public AudioClip GiftButtonCollect;

    [Header("Player Sounds")]
    public AudioClip PlayerStep;
    public AudioClip SelectWeapon;
    public AudioClip Aim;
    public AudioClip Jump;
}