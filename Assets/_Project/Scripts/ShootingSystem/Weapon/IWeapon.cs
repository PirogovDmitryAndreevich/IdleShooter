public interface IWeapon
{
    WeaponType Type { get;}
    WeaponView View { get;}
    ShootShakeSettings ShakeData { get; }
    string AnimationName { get; }
    bool TryShoot(float damage);
}