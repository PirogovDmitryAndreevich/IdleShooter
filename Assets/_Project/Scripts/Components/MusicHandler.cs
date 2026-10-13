using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private ISettingsManager _audioManager;

    private void OnDestroy()
    {
        _audioManager.SoundVolumeChanged -= OnMusicVolumeChanged;
        _audioManager.SoundMuteChanged -= OnMusicMuteChanged;
    }

    public void Construct(ISettingsManager audioManager)
    {
        _audioManager = audioManager;

        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        OnMusicVolumeChanged();
        OnMusicMuteChanged();

        _audioManager.MusicVolumeChanged += OnMusicVolumeChanged;
        _audioManager.MusicMuteChanged += OnMusicMuteChanged;
    }

    private void OnMusicVolumeChanged() => 
        _audioSource.volume = _audioManager.MusicVolume;

    private void OnMusicMuteChanged() => 
        _audioSource.mute = _audioManager.IsMusicMute;
}