using System;

[Serializable]
public class AudioSettingSaveData
{
    public float SoundVolume;
    public bool IsSoundMute;

    public bool IsMusicMute;
    public float MusicVolume;

    public AudioSettingSaveData()
    {
        IsSoundMute = false;
        SoundVolume = 0.5f;

        IsMusicMute = false;
        MusicVolume = 0.5f;
    }
}