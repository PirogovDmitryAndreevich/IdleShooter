public interface IWeapon
{
    WeaponType Type { get;}
    WeaponView View { get;}

    void TryShoot(float damage);
}