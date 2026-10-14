using System;

public class WeaponDataService : IWeaponDataService
{
    private readonly IProgressService _progress;
    private readonly IMetricsService _metrics;

    public event Action<WeaponType> WeaponOpened;

    public WeaponDataService(IProgressService progress, IMetricsService metrics)
    {
        _progress = progress;
        _metrics = metrics;
    }

    public void AddNewWeapon(WeaponType type)
    {
        if (_progress.Data.Weapons.OpenedWeapons.Contains(type))
            return;

        _progress.Data.Weapons.OpenedWeapons.Add(type);

        WeaponOpened?.Invoke(type);

        _metrics.BoughtWeapon(type);
    }

    public void ForInitialize()
    {
        foreach (WeaponType weapon in _progress.Data.Weapons.OpenedWeapons)
            WeaponOpened?.Invoke(weapon);
    }

    public bool GetWeapon(WeaponType type) =>
        _progress.Data.Weapons.OpenedWeapons.Contains(type);
}