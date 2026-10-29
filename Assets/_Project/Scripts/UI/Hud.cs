using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private AimSwitcher _aimSwitcher;
    [SerializeField] private DamagePanel _damagePanel;
    [SerializeField] private PlayerHealthBar _healthBar;
    [SerializeField] private LevelUpUIEffect _levelUp;
    [SerializeField] private Image _aim;
    [SerializeField] private InterstitialTimer _interstitialTimer;
    [SerializeField] private WindowBase[] _windows;
    [SerializeField] private GiftsButton _giftsButton;
    [SerializeField] private WindowButton[] _windowsButtons;
    [SerializeField] private SettingsView _audioSettings;
    [SerializeField] private GiftRewardButton[] _rewardButtons;
    [SerializeField] private WinsPanel _winsPanel;
    [SerializeField] private UserInfo _userInfo;
    [SerializeField] private HudTutorialView _tutorialView;
    [SerializeField] private WeaponWarningPopup _weaponWarning;
    [SerializeField] private ReviewButton _reviewButton;
    [SerializeField] private ReviewPopup _reviewPopup;
    [SerializeField] private SoundsHandler _soundsHandler; 

    private PlayerPresenter _playerPresenter;
    private IUIPoolService _pool;
    private ILevelManager _levelManager;
    private ShootEffectUIAnimation _shootEffectUIAnimation;

    public AimSwitcher AimButton => 
        _aimSwitcher;
    public WindowBase[] Windows =>
        _windows;
    public WindowButton[] WindowsButton =>
        _windowsButtons;
    public SettingsView AudioSettings =>
        _audioSettings;
    public GiftRewardButton[] GiftsRewardButtons =>
            _rewardButtons;
    public WinsPanel WinsPanel =>
        _winsPanel;
    public GiftsButton GiftsButton =>
        _giftsButton;
    public UserInfo UserInfo =>
        _userInfo;
    public HudTutorialView TutorialUI =>
        _tutorialView;
    public WeaponWarningPopup WeaponWarning =>
        _weaponWarning;
    public ReviewButton ReviewButton => _reviewButton;
    public ReviewPopup ReviewPopup => _reviewPopup;
    public SoundsHandler SoundsHandler => _soundsHandler;

    private void OnDestroy()
    {
        _aimSwitcher.AimClicked -= OnAimClicked;
        _playerPresenter.AimChanged -= OnAimChanged;
        _playerPresenter.WeaponAvailabilityChanged -= OnWeaponAvailabilityChanged;
        _playerPresenter.PlayerShot -= OnPlayerShoot;
    }

    public void Construct(PlayerPresenter playerPresenter,
        ShootUIEffectAnimationSettings settings,
        ILevelManager levelManager,
        IUIPoolService pool,
        IInterstitialAdvService interstitialAdvService,
        SoundsData soundsData)
    {
        _playerPresenter = playerPresenter;
        _pool = pool;
        _levelManager = levelManager;

        _shootEffectUIAnimation = new ShootEffectUIAnimation(_aim.rectTransform, settings);

        _interstitialTimer.Construct(interstitialAdvService);
        _damagePanel.Construct(_playerPresenter.DamageManager, _levelManager);
        _healthBar.Construct(_playerPresenter.Health);
        _levelUp.Construct(_healthBar.HealthIcon, _levelManager, _soundsHandler, soundsData);

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
            _aim.gameObject.SetActive(false);        
    }

    public void StartTutorial()
    {
        _giftsButton.gameObject.SetActive(false);
    }

    public void TutorialEnded()
    {
        _giftsButton.gameObject.SetActive(true);
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
        _weaponWarning.View(!hasWeapon);
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