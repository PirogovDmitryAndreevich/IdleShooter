using System;

[Serializable]
public class SettingSaveData
{
    public float SoundVolume;
    public bool IsSoundMute;

    public bool IsMusicMute;
    public float MusicVolume;

    public float Sensitivity;

    public SettingSaveData()
    {
        IsSoundMute = false;
        SoundVolume = 0.5f;

        IsMusicMute = false;
        MusicVolume = 0.5f;

        Sensitivity = 7f;
    }
}