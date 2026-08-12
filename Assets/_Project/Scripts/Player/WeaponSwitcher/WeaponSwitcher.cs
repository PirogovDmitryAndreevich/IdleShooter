using System;
using System.Collections.Generic;

public class WeaponSwitcher : IDisposable
{
    private readonly IGameFactory _gameFactory;
    private readonly IWeaponDataService _weaponData;
    private readonly WeaponsSaveData _weaponsData;
    private readonly Dictionary<WeaponType, IWeapon> _weapons;

    public event Action<IWeapon> WeaponOpened;
    public event Action<IWeapon> WeaponSwitched;

    public IWeapon CurrentWeapon { get; private set; }

    public WeaponSwitcher(IGameFactory gameFactory, IWeaponDataService weaponData)
    {
        _gameFactory = gameFactory;
        _weaponData = weaponData;

        _weapons = new Dictionary<WeaponType, IWeapon>();

        _weaponData.WeaponOpened += OnOpenedWeapon;
    }

    public void SwitchWeapon(WeaponType type)
    {
        if (type == WeaponType.Deselect)
        {
            DeselectWeapon();
            return;
        }

        if (!_weapons.ContainsKey(type))
        {
            UnityEngine.Debug.LogError($"[WeaponSwitcher][SelectWeapon] Попытка выбрать не открытое оружие: {type}");
            return;
        }

        SelectWeapon(type);
    }

    public void Dispose()
    {
        _weaponData.WeaponOpened -= OnOpenedWeapon;
    }

    private void OnOpenedWeapon(WeaponType type)
    {
        if (_weapons.ContainsKey(type))
            return;

        IWeapon weapon = CreateWeapon(type);

        _weapons[type] = weapon;
        WeaponOpened?.Invoke(weapon);
    }

    private void SelectWeapon(WeaponType type)
    {
        CurrentWeapon = _weapons[type];
        WeaponSwitched?.Invoke(CurrentWeapon);
    }

    private void DeselectWeapon()
    {
        CurrentWeapon = null;
        WeaponSwitched?.Invoke(null);
    }

    private IWeapon CreateWeapon(WeaponType type) =>
        _gameFactory.CreateWeapon(type);
}