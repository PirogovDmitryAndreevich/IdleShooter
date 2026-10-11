using System;

public class AudioManager : IAudioManager
{
    private readonly IProgressService _progress;

    public AudioManager(IProgressService progress) =>
        _progress = progress;

    public event Action SoundVolumeChanged;
    public event Action SoundMuteChanged;

    public event Action MusicVolumeChanged;
    public event Action MusicMuteChanged;

    public float SoundVolume =>
        _progress.Data.Audio.SoundVolume;

    public float MusicVolume =>
        _progress.Data.Audio.MusicVolume;

    public bool IsSoundMute =>
        _progress.Data.Audio.IsSoundMute;

    public bool IsMusicMute =>
        _progress.Data.Audio.IsMusicMute;

    public void ChangeSoundVolume(float value)
    {
        if (_progress.Data.Audio.SoundVolume == value)
            return;

        _progress.Data.Audio.SoundVolume = value;

        bool shouldBeMuted = _progress.Data.Audio.SoundVolume <= 0;

        if (IsSoundMute != shouldBeMuted)
            SwitchSoundMute();

        SoundVolumeChanged?.Invoke();
    }

    public void SwitchSoundMute()
    {
        _progress.Data.Audio.IsSoundMute = !IsSoundMute;
        SoundMuteChanged?.Invoke();
    }

    public void ChangeMusicVolume(float value)
    {
        if (_progress.Data.Audio.MusicVolume == value)
            return;

        _progress.Data.Audio.MusicVolume = value;

        bool shouldBeMuted = _progress.Data.Audio.MusicVolume <= 0;

        if (IsMusicMute != shouldBeMuted)
            SwitchMusicMute();

        MusicVolumeChanged?.Invoke();
    }

    public void SwitchMusicMute()
    {
        _progress.Data.Audio.IsMusicMute = !IsMusicMute;
        MusicMuteChanged?.Invoke();
    }
}