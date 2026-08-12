using UnityEngine;

public class SwitchWeaponButton : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _closedMaterial;
    [SerializeField] private Material _openedMaterial;
    [SerializeField] private Material _selectedMaterial;

    private WeaponType _type;
    private WeaponSwitcher _weaponSwitcher;
    private IWeaponDataService _weaponData;
    private bool _isOpened;
    private bool _isSelected;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isOpened)
            return;

        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (_weaponSwitcher.CurrentWeapon.Type == _type)
            return;

        _weaponSwitcher.SwitchWeapon(_type);
    }

    private void OnDestroy()
    {
        _weaponSwitcher.WeaponSwitched -= OnSwitchWeapon;
        _weaponSwitcher.WeaponOpened -= OpenedWeapon;
    }

    public void Construct(WeaponSwitcher weaponSwitcher, IWeaponDataService weaponData, WeaponType type)
    {
        _type = type;
        _weaponSwitcher = weaponSwitcher;
        _weaponData = weaponData;

        _isOpened = _weaponData.GetWeapon(_type);
        _isSelected = _isOpened && _weaponSwitcher.CurrentWeapon.Type == _type;

        _weaponSwitcher.WeaponSwitched += OnSwitchWeapon;
        _weaponSwitcher.WeaponOpened += OpenedWeapon;

        SetMaterial();
    }

    private void OpenedWeapon(IWeapon weapon)
    {
        if (_isOpened || weapon.Type != _type)
            return;

        _isOpened = true;
        SetMaterial();
    }

    private void OnSwitchWeapon(IWeapon weapon)
    {
        if (!_isOpened)
            return;

        _isSelected = weapon.Type == _type;
        SetMaterial();
    }

    private void SetMaterial()
    { 
        if (!_isOpened)
        {
            _meshRenderer.sharedMaterial = _closedMaterial;
            return;
        }

        _meshRenderer.sharedMaterial =
            _isSelected ? _selectedMaterial : _openedMaterial;
    }
}