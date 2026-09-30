using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private AimSwitcher _aimSwitcher;
    [SerializeField] private DamagePanel _damagePanel;
    [SerializeField] private PlayerHealthBar _healthBar;
    [SerializeField] private LevelUpUIEffect _levelUp;
    [SerializeField] private Image _aim;

    private PlayerPresenter _playerPresenter;
    private IUIPoolService _pool;
    private ILevelManager _levelManager;
    private ShootEffectUIAnimation _shootEffectUIAnimation;

    private void OnDestroy()
    {
        _aimSwitcher.AimClicked -= OnAimClicked;
        _playerPresenter.AimChanged -= OnAimChanged;
        _playerPresenter.WeaponAvailabilityChanged -= OnWeaponAvailabilityChanged;
        _playerPresenter.PlayerShot -= OnPlayerShoot;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            OnPlayerLevelUp(0);
    }

    public void Construct(PlayerPresenter playerPresenter,
        ShootUIEffectAnimationSettings settings,
        ILevelManager levelManager,
        IUIPoolService pool)
    {
        _playerPresenter = playerPresenter;
        _pool = pool;
        _levelManager = levelManager;

        _shootEffectUIAnimation = new ShootEffectUIAnimation(_aim.rectTransform, settings);

        _damagePanel.Construct(_playerPresenter.DamageManager, _levelManager);
        _healthBar.Construct(_playerPresenter.Health);
        _levelUp.Construct(_healthBar.HealthIcon);

        _aim.gameObject.SetActive(false);

        _aimSwitcher.AimClicked += OnAimClicked;
        _playerPresenter.AimChanged += OnAimChanged;
        _playerPresenter.WeaponAvailabilityChanged += OnWeaponAvailabilityChanged;
        _playerPresenter.PlayerShot += OnPlayerShoot;
        _levelManager.LevelChanged += OnPlayerLevelUp;

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

    private void OnPlayerShoot(float score)
    {
        ShootUIEffect effect = _pool.Get<ShootUIEffect>();

        string scoreText = NumberFormatter.NumberAsString(score);

        effect.Show(scoreText);

        _shootEffectUIAnimation.StartAnimation(effect);
    }

    private void OnPlayerLevelUp(int level) => 
        _levelUp.Show();
}
