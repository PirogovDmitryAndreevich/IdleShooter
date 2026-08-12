using UnityEngine;

[CreateAssetMenu(fileName ="GameStaticData", menuName = "StaticData/GameData")]
public class GameStaticData : ScriptableObject
{
    public CameraSettingsData CameraSettingsData;

    [Space]
    [Header("Weapon Settings")]
    [Tooltip("Маски коллизии для оружия")]
    public LayerMask AimLayerMask;

    [Space]
    public WeaponSwitcherData[] WeaponSwitcherData;
}
