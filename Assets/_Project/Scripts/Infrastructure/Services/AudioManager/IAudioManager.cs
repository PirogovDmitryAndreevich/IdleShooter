using System;

public interface IAudioManager : IService
{
    bool IsMusicMute { get; }
    bool IsSoundMute { get; }
    float MusicVolume { get; }
    float SoundVolume { get; }

    event Action MusicMuteChanged;
    event Action MusicVolumeChanged;
    event Action SoundMuteChanged;
    event Action SoundVolumeChanged;

    void ChangeMusicVolume(float value);
    void ChangeSoundVolume(float value);
    void SwitchMusicMute();
    void SwitchSoundMute();
}