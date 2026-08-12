using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _freeCameraTarget;
    [SerializeField] private Transform _aimCameraTarget;
    [SerializeField] private Transform _weaponSpawnParent;
    [SerializeField] private CharacterController _characterController;

    public Animator Animator => _animator;
    public Transform FreeCameraTarget => _freeCameraTarget;
    public Transform AimCameraTarget => _aimCameraTarget;
    public CharacterController CharacterController => _characterController;
    public Transform Transform => transform;
    public Transform WeaponSpawnParent => _weaponSpawnParent;

    private Dictionary<WeaponType, WeaponView> _weapons;
    private WeaponView _currentWeapon;

    public void Construct()
    {
        _weapons = new();
    }

    public void OpenWeapon(IWeapon weapon)
    {
        if (_weapons.ContainsKey(weapon.Type))
            return;

        WeaponView weaponView = weapon.View;

        _weapons.Add(weapon.Type, weaponView);

        weaponView.transform.SetParent(_weaponSpawnParent);
        weaponView.transform.localPosition = Vector3.zero;
        weaponView.transform.localRotation = Quaternion.identity;

        weaponView.gameObject.SetActive(false);
    }

    public void SwitchWeapon(IWeapon weapon)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = null;
        }

        if (weapon == null)
            return;

        if (!_weapons.TryGetValue(
                weapon.Type,
                out WeaponView weaponView))
            return;

        _currentWeapon = weaponView;
        _currentWeapon.gameObject.SetActive(true);
    }
}