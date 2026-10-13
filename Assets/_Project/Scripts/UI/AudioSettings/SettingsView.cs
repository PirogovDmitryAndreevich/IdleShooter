using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sensitivitySlider;

    [Header("Buttons")]
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _musicButton;

    [Header("Sprites")]
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [Space]
    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;

    [Header("Images")]
    [SerializeField] private Image _soundImage;
    [SerializeField] private Image _musicImage;

    private ISettingsManager _settingsManager;

    private void OnDestroy()
    {
        _soundSlider.onValueChanged.RemoveListener(ChangedSoundVolume);
        _musicSlider.onValueChanged.RemoveListener(ChangedMusicVolume);
        _sensitivitySlider.onValueChanged.RemoveListener(ChangeSensitivityValue);

        _soundButton.onClick.RemoveListener(SwitchMuteSound);
        _musicButton.onClick.RemoveListener(SwitchMuteMusic);
    }

    public void Construct(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;

        _sensitivitySlider.minValue = 1f;
        _sensitivitySlider.maxValue = 10f;
        _sensitivitySlider.wholeNumbers = false;

        _soundSlider.onValueChanged.AddListener(ChangedSoundVolume);
        _musicSlider.onValueChanged.AddListener(ChangedMusicVolume);
        _sensitivitySlider.onValueChanged.AddListener(ChangeSensitivityValue);

        _soundButton.onClick.AddListener(SwitchMuteSound);
        _musicButton.onClick.AddListener(SwitchMuteMusic);

        ResetSettings();
    }

    private void ChangedSoundVolume(float value)
    {
        _settingsManager.ChangeSoundVolume(value);

        if (value <= 0f)
        {
            if (!_settingsManager.IsSoundMute)
                _settingsManager.SwitchSoundMute();
        }
        else
        {
            if (_settingsManager.IsSoundMute)
                _settingsManager.SwitchSoundMute();
        }

        _soundImage.sprite = _settingsManager.IsSoundMute ? _soundOff : _soundOn;
    }

    private void ChangedMusicVolume(float value)
    {
        _settingsManager.ChangeMusicVolume(value);

        if (value <= 0f)
        {
            if (!_settingsManager.IsMusicMute)
                _settingsManager.SwitchMusicMute();
        }
        else
        {
            if (_settingsManager.IsMusicMute)
                _settingsManager.SwitchMusicMute();
        }

        _musicImage.sprite = _settingsManager.IsMusicMute ? _musicOff : _musicOn;
    }

    private void ChangeSensitivityValue(float value) => 
        _settingsManager.ChangeSensitivity(value);

    private void SwitchMuteSound()
    {
        _settingsManager.SwitchSoundMute();
        SetSoundSprite();
    }

    private void SwitchMuteMusic()
    {
        _settingsManager.SwitchMusicMute();
        SetMusicSprite();
    }

    private void SetSoundSprite() => 
        _soundImage.sprite =
        _settingsManager.IsSoundMute
        ? _soundOff
        : _soundOn;

    private void SetMusicSprite() => 
        _musicImage.sprite =
        _settingsManager.IsMusicMute
        ? _musicOff 
        : _musicOn;

    private void ResetSettings()
    {
        _soundSlider.SetValueWithoutNotify(_settingsManager.SoundVolume);
        _musicSlider.SetValueWithoutNotify(_settingsManager.MusicVolume);
        _sensitivitySlider.SetValueWithoutNotify(_settingsManager.Sensitivity);
        SetSoundSprite();
        SetMusicSprite();
    }
}