using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _musicButton;

    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;

    [SerializeField] private Image _soundImage;
    [SerializeField] private Image _musicImage;

    private IAudioManager _audioManager;

    private void OnDestroy()
    {
        _soundSlider.onValueChanged.RemoveListener(ChangedSoundVolume);
        _musicSlider.onValueChanged.RemoveListener(ChangedMusicVolume);

        _soundButton.onClick.RemoveListener(SwitchMuteSound);
        _musicButton.onClick.RemoveListener(SwitchMuteMusic);
    }

    public void Construct(IAudioManager audioManager)
    {
        _audioManager = audioManager;

        _soundSlider.onValueChanged.AddListener(ChangedSoundVolume);
        _musicSlider.onValueChanged.AddListener(ChangedMusicVolume);

        _soundButton.onClick.AddListener(SwitchMuteSound);
        _musicButton.onClick.AddListener(SwitchMuteMusic);

        ResetSettings();
    }

    private void ChangedSoundVolume(float value)
    {
        _audioManager.ChangeSoundVolume(value);

        if (value <= 0f)
        {
            if (!_audioManager.IsSoundMute)
                _audioManager.SwitchSoundMute();
        }
        else
        {
            if (_audioManager.IsSoundMute)
                _audioManager.SwitchSoundMute();
        }

        _soundImage.sprite = _audioManager.IsSoundMute ? _soundOff : _soundOn;
    }

    private void ChangedMusicVolume(float value)
    {
        _audioManager.ChangeMusicVolume(value);

        if (value <= 0f)
        {
            if (!_audioManager.IsMusicMute)
                _audioManager.SwitchMusicMute();
        }
        else
        {
            if (_audioManager.IsMusicMute)
                _audioManager.SwitchMusicMute();
        }

        _musicImage.sprite = _audioManager.IsMusicMute ? _musicOff : _musicOn;
    }

    private void SwitchMuteSound()
    {
        _audioManager.SwitchSoundMute();
        SetSoundSprite();
    }

    private void SwitchMuteMusic()
    {
        _audioManager.SwitchMusicMute();
        SetMusicSprite();
    }

    private void SetSoundSprite() => 
        _soundImage.sprite =
        _audioManager.IsSoundMute
        ? _soundOff
        : _soundOn;

    private void SetMusicSprite() => 
        _musicImage.sprite =
        _audioManager.IsMusicMute
        ? _musicOff 
        : _musicOn;

    private void ResetSettings()
    {
        _soundSlider.SetValueWithoutNotify(_audioManager.SoundVolume);
        _musicSlider.SetValueWithoutNotify(_audioManager.MusicVolume);
        SetSoundSprite();
        SetMusicSprite();
    }
}