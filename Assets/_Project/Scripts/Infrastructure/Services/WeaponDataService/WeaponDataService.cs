using System;
using System.Collections.Generic;
using System.Linq;

public class WeaponDataService : IWeaponDataService
{
    private readonly IProgressService _progress;
    private readonly IMetricsService _metrics;
    private readonly ILevelManager _levelManager;

    private readonly Dictionary<WeaponType, WeaponStaticData> _weapons;

    public event Action<WeaponType> WeaponOpened;

    public WeaponDataService(IProgressService progress, IMetricsService metrics,
        ILevelManager levelManager, IStaticDataService staticDataService)
    {
        _progress = progress;
        _metrics = metrics;
        _levelManager = levelManager;

        _weapons = staticDataService.GetGameData().WeaponsStaticData
            .ToDictionary(x => x.Type, x => x);
    }

    public void TryBuyWeapon(WeaponType type)
    {
        if (!_weapons.TryGetValue(type, out WeaponStaticData weapon))
            return;

        if (_levelManager.Wins < weapon.Price)
            return;

        _levelManager.SpendWins(weapon.Price);
        AddNewWeapon(type);
    }

    public void ForInitialize()
    {
        foreach (WeaponType weapon in _progress.Data.Weapons.OpenedWeapons)
            WeaponOpened?.Invoke(weapon);
    }

    public bool GetWeapon(WeaponType type) =>
        _progress.Data.Weapons.OpenedWeapons.Contains(type);

    private void AddNewWeapon(WeaponType type)
    {
        if (_progress.Data.Weapons.OpenedWeapons.Contains(type))
            return;

        _progress.Data.Weapons.OpenedWeapons.Add(type);

        WeaponOpened?.Invoke(type);

        _metrics.BoughtWeapon(type);
    }
}