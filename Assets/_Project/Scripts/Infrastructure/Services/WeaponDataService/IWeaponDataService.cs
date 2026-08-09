using System;

public interface IWeaponDataService : IService
{
    event Action<WeaponType> WeaponOpened;

    void AddNewWeapon(WeaponType type);
    void ForInitialize();
}