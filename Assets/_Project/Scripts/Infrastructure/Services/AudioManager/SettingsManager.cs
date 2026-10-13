using System;

public class SettingsManager : ISettingsManager
{
    private readonly IProgressService _progress;

    public SettingsManager(IProgressService progress) =>
        _progress = progress;

    public event Action SoundVolumeChanged;
    public event Action SoundMuteChanged;
    public event Action SensitivityChanged;

    public event Action MusicVolumeChanged;
    public event Action MusicMuteChanged;

    public float SoundVolume =>
        _progress.Data.Settings.SoundVolume;

    public float MusicVolume =>
        _progress.Data.Settings.MusicVolume;

    public bool IsSoundMute =>
        _progress.Data.Settings.IsSoundMute;

    public bool IsMusicMute =>
        _progress.Data.Settings.IsMusicMute;

    public float Sensitivity =>
        _progress.Data.Settings.Sensitivity;

    public void ChangeSoundVolume(float value)
    {
        if (_progress.Data.Settings.SoundVolume == value)
            return;

        _progress.Data.Settings.SoundVolume = value;

        bool shouldBeMuted = _progress.Data.Settings.SoundVolume <= 0;

        if (IsSoundMute != shouldBeMuted)
            SwitchSoundMute();

        SoundVolumeChanged?.Invoke();
    }

    public void SwitchSoundMute()
    {
        _progress.Data.Settings.IsSoundMute = !IsSoundMute;
        SoundMuteChanged?.Invoke();
    }

    public void ChangeMusicVolume(float value)
    {
        if (_progress.Data.Settings.MusicVolume == value)
            return;

        _progress.Data.Settings.MusicVolume = value;

        bool shouldBeMuted = _progress.Data.Settings.MusicVolume <= 0;

        if (IsMusicMute != shouldBeMuted)
            SwitchMusicMute();

        MusicVolumeChanged?.Invoke();
    }

    public void SwitchMusicMute()
    {
        _progress.Data.Settings.IsMusicMute = !IsMusicMute;
        MusicMuteChanged?.Invoke();
    }

    public void ChangeSensitivity(float value)
    {
        if (_progress.Data.Settings.Sensitivity == value)
            return;

        _progress.Data.Settings.Sensitivity = value;

        SensitivityChanged?.Invoke();
    }
}