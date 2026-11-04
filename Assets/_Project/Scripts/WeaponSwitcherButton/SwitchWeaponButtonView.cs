using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SwitchWeaponButtonView : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Image _icon; 
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _bonusText;
    [SerializeField] private TMP_Text _priceText;

    [Header("Localization")]
    [SerializeField] private LocalizedString _bonusLocale;
    [SerializeField] private LocalizedString _priceLocale;
    [SerializeField] private LocalizedStringTable _localizedTable;

    [Header("Mesh Settings")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _closedMaterial;
    [SerializeField] private Material _openedMaterial;
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private Material _canBuyMaterial;

    private SwitchWeaponButtonModel _model;
    private LocalizedString _nameLocale;

    public event Action Dispose;
    public event Action PlayerTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        PlayerTrigger?.Invoke();
    }

    private void OnDestroy()
    {
        if (_bonusLocale != null)
            _bonusLocale.StringChanged -= UpdateBonusText;

        if (_priceLocale != null)
            _priceLocale.StringChanged -= UpdatePriceText;

        if (_nameLocale != null)
            _nameLocale.StringChanged -= UpdateNameText;

        Dispose?.Invoke();
    }

    public void Construct(SwitchWeaponButtonModel model)
    {
        _model = model;

        _icon.sprite = _model.Icon;

        _nameLocale = new LocalizedString(_localizedTable.TableReference, _model.Type.ToString());

        _nameLocale.StringChanged += UpdateNameText;
        _bonusLocale.StringChanged += UpdateBonusText;
        _priceLocale.StringChanged += UpdatePriceText;
    }

    public void SetOpenedState(bool isSelected)
    {
        _priceText.gameObject.SetActive(false);

        _meshRenderer.sharedMaterial =
        isSelected ? _selectedMaterial : _openedMaterial;
    }

    public void NoOpenedState() => 
        _meshRenderer.sharedMaterial = _closedMaterial;

    public void SetCanBayState() => 
        _meshRenderer.sharedMaterial = _canBuyMaterial;

    private void UpdateBonusText(string value) =>
        _bonusText.text = string.Format(value, _model.Score);

    private void UpdatePriceText(string value) =>
        _priceText.text = string.Format(value, _model.Price);

    private void UpdateNameText(string value) =>
        _name.text = value;
}
