using TMPro;
using UnityEngine;

public class SwitchWeaponButton : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _bonusText;
    [SerializeField] private TMP_Text _priceText;

    [Header("Mesh Settings")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _closedMaterial;
    [SerializeField] private Material _openedMaterial;
    [SerializeField] private Material _selectedMaterial;

    private WeaponType _type;
    private WeaponSwitcher _weaponSwitcher;
    private IWeaponDataService _weaponsDataService;
    private WeaponStaticData _weaponData;
    private bool _isOpened;
    private bool _isSelected;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isOpened)
            return;

        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        _weaponSwitcher.SwitchWeapon(_type);
    }

    private void OnDestroy()
    {
        _weaponSwitcher.WeaponSwitched -= OnSwitchWeapon;
        _weaponSwitcher.WeaponOpened -= OpenedWeapon;
    }

    public void Construct(WeaponSwitcher weaponSwitcher, IWeaponDataService weaponsDataService,
        WeaponType type, WeaponStaticData weaponData)
    {
        _type = type;
        _weaponSwitcher = weaponSwitcher;
        _weaponsDataService = weaponsDataService;
        _weaponData = weaponData;

        _name.text = type.ToString();
        _bonusText.text = $"+{_weaponData.Score}";
        _priceText.text = $"требуется {_weaponData.Price}";

        _isOpened = _weaponsDataService.GetWeapon(_type);

        _weaponSwitcher.WeaponSwitched += OnSwitchWeapon;
        _weaponSwitcher.WeaponOpened += OpenedWeapon;

        SetMaterial();
    }

    private void OpenedWeapon(IWeapon weapon)
    {
        if (_isOpened || weapon.Type != _type)
            return;

        _isOpened = true;
        _priceText.gameObject.SetActive(false);
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