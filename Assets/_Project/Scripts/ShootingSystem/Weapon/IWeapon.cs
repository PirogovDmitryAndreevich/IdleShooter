public interface IWeapon
{
    WeaponType Type { get;}
    WeaponView View { get;}
    ShootShakeSettings Shake { get; }
    string AnimationName { get; }

    bool TryShoot(float damage);
}