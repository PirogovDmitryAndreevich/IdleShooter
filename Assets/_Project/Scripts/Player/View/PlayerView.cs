using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayer
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _freeCameraTarget;
    [SerializeField] private Transform _aimCameraTarget;
    [SerializeField] private Transform _weaponSpawnParent;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private LineDraw _lineDraw;

    private WeaponView _currentWeapon;

    public event Action PlayerDied;
    public event Action<float, Vector3, Vector3> Damage;
    public event Action<IPassiveShooting> PassiveShooterInteract;
    public event Action StopPassiveShooting;

    private Dictionary<WeaponType, WeaponView> _weapons;

    public Animator Animator => _animator;
    public Transform FreeCameraTarget => _freeCameraTarget;
    public Transform AimCameraTarget => _aimCameraTarget;
    public CharacterController CharacterController => _characterController;
    public Transform Transform => transform;
    public Transform WeaponSpawnParent => _weaponSpawnParent;
    public HitFlash HitEffect => _hitEffect;
    public LineDraw LineDraw => _lineDraw;

    private void OnDestroy() => 
        _damageableCollider.DamageReceived -= TakeDamage;

    public void Construct()
    {
        _weapons = new();
        _damageableCollider.DamageReceived += TakeDamage;
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

    public void Died() => 
        PlayerDied?.Invoke();

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) => 
        Damage?.Invoke(damage, hitPoint, hitNormal);

    public void TakeDamage(float damage) => 
        TakeDamage(damage, transform.position, Vector3.up);

    public void PassiveShooting(IPassiveShooting passiveShooting) => 
        PassiveShooterInteract?.Invoke(passiveShooting);

    public void OnStopPassiveShooting() =>
        StopPassiveShooting?.Invoke();
}