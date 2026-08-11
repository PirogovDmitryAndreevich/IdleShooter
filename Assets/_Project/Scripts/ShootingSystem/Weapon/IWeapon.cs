public interface IWeapon
{
    WeaponType Type { get;}
    WeaponView View { get;}
    ShootShakeSettings Shake { get; }

    bool TryShoot(float damage);
}