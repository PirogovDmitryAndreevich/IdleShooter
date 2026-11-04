using UnityEngine;

public class SwitchWeaponButtonModel
{
    private readonly WeaponStaticData _weaponData;
    private readonly IProgressService _progress;

    public SwitchWeaponButtonModel(WeaponStaticData weaponData, IProgressService progress)
    {
        _weaponData = weaponData;
        _progress = progress;
    }

    public WeaponType Type => _weaponData.Type;

    public int Price => _weaponData.Price;

    public float Score => _weaponData.Score;

    public bool IsOpened =>
        _progress.Data.Weapons.OpenedWeapons.Contains(Type);

    public bool CanBuy =>
        !IsOpened &&
        _progress.Data.PlayerData.Wins >= Price;

    public bool IsSelected { get; private set; } = false;

    public Sprite Icon => _weaponData.Icon;

    public void SetSelectedState(bool isSelected) =>
        IsSelected = isSelected;
}
