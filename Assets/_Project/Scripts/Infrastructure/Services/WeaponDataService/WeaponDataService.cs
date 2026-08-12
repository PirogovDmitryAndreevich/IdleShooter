using System;

public class WeaponDataService : IWeaponDataService
{
    private readonly IProgressService _progress;

    public event Action<WeaponType> WeaponOpened;

    public WeaponDataService(IProgressService progress)
    {
        _progress = progress;
    }

    public void AddNewWeapon(WeaponType type)
    {
        if (_progress.Data.Weapons.OpenedWeapons.Contains(type))
            return;

        _progress.Data.Weapons.OpenedWeapons.Add(type);

        WeaponOpened?.Invoke(type);
    }

    public void ForInitialize()
    {
        foreach (WeaponType weapon in _progress.Data.Weapons.OpenedWeapons)
            WeaponOpened?.Invoke(weapon);
    }

    public bool GetWeapon(WeaponType type) =>
        _progress.Data.Weapons.OpenedWeapons.Contains(type);
}