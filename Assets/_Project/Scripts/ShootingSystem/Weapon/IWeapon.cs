public interface IWeapon
{
    WeaponType Type { get;}
    WeaponView View { get;}
    ShootShakeSettings ShakeData { get; }
    string AnimationName { get; }
    float Score { get; }
    bool TryShoot(float damage);
}