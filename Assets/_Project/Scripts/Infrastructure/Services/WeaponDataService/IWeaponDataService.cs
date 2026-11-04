using System;

public interface IWeaponDataService : IService
{
    event Action<WeaponType> WeaponOpened;

    void TryBuyWeapon(WeaponType type);
    void ForInitialize();
    bool GetWeapon(WeaponType type);
}