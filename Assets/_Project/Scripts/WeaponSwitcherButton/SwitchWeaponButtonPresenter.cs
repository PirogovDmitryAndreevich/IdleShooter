using UnityEngine;

public class SwitchWeaponButtonPresenter
{
    private readonly SwitchWeaponButtonView _view;
    private readonly SwitchWeaponButtonModel _model;
    private readonly WeaponSwitcher _weaponSwitcher;
    private readonly IWeaponDataService _weaponDataService;
    private readonly ILevelManager _levelManager;

    public SwitchWeaponButtonPresenter(SwitchWeaponButtonView view, SwitchWeaponButtonModel model,
        WeaponSwitcher weaponSwitcher, IWeaponDataService weaponDataService, ILevelManager levelManager)
    {
        _view = view;
        _model = model;
        _weaponSwitcher = weaponSwitcher;
        _weaponDataService = weaponDataService;
        _levelManager = levelManager;

        _view.Dispose += OnDispose;
        _view.PlayerTrigger += OnPlayerTrigger;

        _weaponSwitcher.WeaponSwitched += OnWeaponSwitched;

        _weaponDataService.WeaponOpened += OnWeaponOpened;

        _levelManager.WinsChanged += OnWinsChanged;

        SetView();
    }

    public WeaponType Type => _model.Type;
    public bool CanBuy => _model.CanBuy;
    public bool IsSelected => _model.IsSelected;
    public Transform Transform => _view.transform;

    private void OnDispose()
    {
        if (_view != null)
        {
            _view.Dispose -= OnDispose;
            _view.PlayerTrigger -= OnPlayerTrigger;
        }

        if (_weaponSwitcher != null)        
            _weaponSwitcher.WeaponSwitched -= OnWeaponSwitched;    
        
        if(_weaponDataService != null)
            _weaponDataService.WeaponOpened -= OnWeaponOpened;

        if(_levelManager != null)
            _levelManager.WinsChanged -= OnWinsChanged;
    }

    private void OnPlayerTrigger()
    {
        if (!_model.IsOpened && !_model.CanBuy)
            return;

        if (_model.CanBuy)
            Buy();

        if (_model.IsOpened && !_model.IsSelected)
            _weaponSwitcher.SwitchWeapon(_model.Type);
    }    

    private void OnWeaponSwitched(IWeapon weapon)
    {
        _model.SetSelectedState(weapon.Type == _model.Type);
        SetView();
    }

    private void OnWeaponOpened(WeaponType type)
    {
        if (type != _model.Type)
            return;

        SetView();
    }

    private void OnWinsChanged(int value) => 
        SetView();

    private void Buy()
    {
        _weaponDataService.TryBuyWeapon(_model.Type);
        SetView();
    }

    private void SetView()
    {
        if (_model.IsOpened)
        {
            _view.SetOpenedState(_model.IsSelected);
            return;
        }

        if (_model.CanBuy)
        {
            _view.SetCanBayState();
            return;
        }

        _view.NoOpenedState();
    }
}