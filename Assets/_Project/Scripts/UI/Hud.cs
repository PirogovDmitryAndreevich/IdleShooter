using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TMP_Text _weaponName;
    [SerializeField] private Transform _buttonParent;

    private WeaponSwitcher _weaponSwitcher;
    private IGameFactory _gameFactory;

    private void OnDestroy()
    {
        _weaponSwitcher.WeaponSwitched -= OnWeaponSelected;
        _weaponSwitcher.WeaponOpened -= OnOpenedNewWeapon;
    }

    public void Construct(WeaponSwitcher weaponSwitcher, IGameFactory gameFactory)
    {
        _weaponSwitcher = weaponSwitcher;
        _gameFactory = gameFactory;

        _weaponSwitcher.WeaponSwitched += OnWeaponSelected;
        _weaponSwitcher.WeaponOpened += OnOpenedNewWeapon;

        OnWeaponSelected(null);
    }

    public void SwitchWeapon(WeaponType type)
    {
        IWeapon currentWeapon = _weaponSwitcher.CurrentWeapon;

        if (currentWeapon != null && currentWeapon.Type == type)
        {
            _weaponSwitcher.SwitchWeapon(WeaponType.Deselect);
            return;
        }

        _weaponSwitcher.SwitchWeapon(type);
    }

    public void OnWeaponSelected(IWeapon type)
    {
        if (type == null)
        {
            _weaponName.text = WeaponType.Deselect.ToString();
            return;
        }

        _weaponName.text = _weaponSwitcher.CurrentWeapon.Type.ToString();
    }

    public void OnOpenedNewWeapon(IWeapon weapon)
    {
        SelectWeaponButton button = _gameFactory.CreateWeaponSelectButton(_buttonParent);
        button.Construct(this, weapon);
    }
}
