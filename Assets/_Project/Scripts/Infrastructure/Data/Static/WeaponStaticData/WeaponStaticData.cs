using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "StaticData/WeaponData")]
public class WeaponStaticData : ScriptableObject
{
    public WeaponType Type;
    public ShooterType ShooterType;
    [Tooltip("Опционально. Если ShooterType - Projectile")]
    public ProjectileType ProjectileType;
    public float FireRate = 0.2f;
    public float MaxAimDistance = 1000;
    public ShootShakeSettings Shake;
    public string AnimationName;
}
