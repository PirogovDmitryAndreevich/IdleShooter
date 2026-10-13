using System;

public interface ISettingsManager : IService
{
    bool IsMusicMute { get; }
    bool IsSoundMute { get; }
    float MusicVolume { get; }
    float SoundVolume { get; }
    float Sensitivity { get; }

    event Action MusicMuteChanged;
    event Action MusicVolumeChanged;
    event Action SoundMuteChanged;
    event Action SoundVolumeChanged;
    event Action SensitivityChanged;

    void ChangeMusicVolume(float value);
    void ChangeSoundVolume(float value);
    void ChangeSensitivity(float value);
    void SwitchMusicMute();
    void SwitchSoundMute();
}