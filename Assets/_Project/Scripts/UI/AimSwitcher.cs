using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class AimSwitcher : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _viewSprite;
    [SerializeField] private TMP_Text _keyCodeText;

    private Sprite _aimSprite;
    private IInputService _inputService;
    private ButtonsKeyCodes _buttonsKeyCodes;

    public bool IsAim { get; private set; }

    private void OnEnable() => 
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

    private void OnDisable() => 
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

    private void OnDestroy() => 
        _button.onClick.RemoveListener(OnClick);

    public void Construct(IInputService inputService, ButtonsKeyCodes buttonsKeyCodes)
    {
        _inputService = inputService;
        _buttonsKeyCodes = buttonsKeyCodes;

        UpdateKeyCodeText();

        IsAim = false;

        _button.onClick.AddListener(OnClick);
    }

    public void SetAimState(bool isAim)
    {
        IsAim = isAim;

        _image.sprite = IsAim
            ? _aimSprite
            : _viewSprite;
    }

    public void SetIcon(Sprite icon) =>
        _aimSprite = icon;

    private void OnClick() => 
        _inputService.SwitchButtonPressed();

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale) => 
        UpdateKeyCodeText();

    private void UpdateKeyCodeText()
    {
        if (_keyCodeText == null || _buttonsKeyCodes == null)
            return;

        _keyCodeText.text = LocalizedKeyCode.GetLocalizedKeyCode(
            _buttonsKeyCodes.SwitchAim);
    }
}