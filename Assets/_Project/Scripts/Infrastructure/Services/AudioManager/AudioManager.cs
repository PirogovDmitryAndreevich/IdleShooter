using System;

public class AudioManager : IAudioManager
{
    private readonly AudioSettingSaveData _saveData;

    public AudioManager(IProgressService progress) =>
        _saveData = progress.Data.Audio;

    public event Action SoundVolumeChanged;
    public event Action SoundMuteChanged;

    public event Action MusicVolumeChanged;
    public event Action MusicMuteChanged;

    public float SoundVolume =>
        _saveData.SoundVolume;

    public float MusicVolume =>
        _saveData.MusicVolume;

    public bool IsSoundMute =>
        _saveData.IsSoundMute;

    public bool IsMusicMute =>
        _saveData.IsMusicMute;

    public void ChangeSoundVolume(float value)
    {
        if (_saveData.SoundVolume == value)
            return;

        _saveData.SoundVolume = value;

        bool shouldBeMuted = _saveData.SoundVolume <= 0;

        if (IsSoundMute != shouldBeMuted)
            SwitchSoundMute();

        SoundVolumeChanged?.Invoke();
    }

    public void SwitchSoundMute()
    {
        _saveData.IsSoundMute = !IsSoundMute;
        SoundMuteChanged?.Invoke();
    }

    public void ChangeMusicVolume(float value)
    {
        if (_saveData.MusicVolume == value)
            return;

        _saveData.MusicVolume = value;

        bool shouldBeMuted = _saveData.MusicVolume <= 0;

        if (IsMusicMute != shouldBeMuted)
            SwitchMusicMute();

        MusicVolumeChanged?.Invoke();
    }

    public void SwitchMusicMute()
    {
        _saveData.IsMusicMute = !IsMusicMute;
        MusicMuteChanged?.Invoke();
    }
}