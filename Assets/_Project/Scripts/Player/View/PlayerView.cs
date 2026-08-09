using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _weaponSpawnParent;
    [SerializeField] private CharacterController _characterController;

    public Animator Animator => _animator;
    public Transform CameraTarget => _cameraTarget;
    public CharacterController CharacterController => _characterController;
    public Transform Transform => transform;
    public Transform WeaponSpawnParent => _weaponSpawnParent;

    private Dictionary<WeaponType, WeaponView> _weapons;
    private WeaponSwitcher _switcher;
    private WeaponView _currentWeapon;

    private void OnDestroy()
    {
        _switcher.WeaponSwitched -= OnWeaponSwitched;
        _switcher.WeaponOpened -= OnWeaponOpened;
    }

    public void Construct(WeaponSwitcher weaponSwitcher)
    {
        _weapons = new();
        _switcher = weaponSwitcher;

        _switcher.WeaponSwitched += OnWeaponSwitched;
        _switcher.WeaponOpened += OnWeaponOpened;
    }

    public void OnWeaponSwitched(IWeapon weapon)
    {
        if (weapon == null)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.gameObject.SetActive(false);
                _currentWeapon = null;
            }

            return;
        }

        if (!_weapons.ContainsKey(weapon.Type))
            return;

        _currentWeapon = _weapons[weapon.Type];
        _currentWeapon.gameObject.SetActive(true);
    }

    public void OnWeaponOpened(IWeapon weapon)
    {
        if (_weapons.ContainsKey(weapon.Type))
            return;

        WeaponView newWeapon = weapon.View;

        _weapons[weapon.Type] = newWeapon;
        newWeapon.transform.position = _weaponSpawnParent.position;
        newWeapon.transform.SetParent(_weaponSpawnParent);

        newWeapon.gameObject.SetActive(false);
    }

}