using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private AimSwitcher _aimSwitcher;
    [SerializeField] private DamagePanel _damagePanel;
    [SerializeField] private Image _aim; 

    private PlayerPresenter _playerPresenter;

    private void OnDestroy()
    {
        _aimSwitcher.AimClicked -= OnAimClicked;
        _playerPresenter.AimChanged -= OnAimChanged;
        _playerPresenter.WeaponAvailabilityChanged -= OnWeaponAvailabilityChanged;
    }

    public void Construct(PlayerPresenter playerPresenter, ILevelManager levelManager)
    {
        _playerPresenter = playerPresenter;

        _damagePanel.Construct(_playerPresenter.DamageManager, levelManager);

        _aim.gameObject.SetActive(false);

        _aimSwitcher.AimClicked += OnAimClicked;
        _playerPresenter.AimChanged += OnAimChanged;
        _playerPresenter.WeaponAvailabilityChanged += OnWeaponAvailabilityChanged;

        SetAimSwitcherVisible(false);
    }

    public void SetAimSwitcherVisible(bool visible)
    {
        _aimSwitcher.gameObject.SetActive(visible);

        if (!visible)
        {
            _aim.gameObject.SetActive(false);
        }
    }

    private void OnAimClicked()
    {
        _playerPresenter.SwitchAim(
            !_playerPresenter.IsAiming);
    }

    private void OnAimChanged(bool isAim)
    {
        _aimSwitcher.SetAimState(isAim);
        _aim.gameObject.SetActive(isAim);
    }

    private void OnWeaponAvailabilityChanged(bool hasWeapon)
    {
        SetAimSwitcherVisible(hasWeapon);
    }
}
