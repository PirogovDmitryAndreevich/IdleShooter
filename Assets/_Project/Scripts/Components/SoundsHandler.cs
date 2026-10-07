using System;
using UnityEngine;

public class SoundsHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Space]
    [SerializeField] private float _lowPith = 0f;
    [SerializeField] private float _topPith = 2f;

    private IAudioManager _audioManager;

    private void OnDestroy()
    {
        _audioManager.SoundVolumeChanged -= OnSoundVolumeChanged;
        _audioManager.SoundMuteChanged -= OnSoundMuteChanged;
    }

    public void Construct(IAudioManager audioManager)
    {
        _audioManager = audioManager;

        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        OnSoundVolumeChanged();
        OnSoundMuteChanged();

        _audioManager.SoundVolumeChanged += OnSoundVolumeChanged;
        _audioManager.SoundMuteChanged += OnSoundMuteChanged;
    }

    public void PlaySound(AudioClip clip) =>
        _audioSource.PlayOneShot(clip);

    public void PlayRandomPithSound(AudioClip clip)
    {
        float currentPitch = _audioSource.pitch;
        float pitch = UnityEngine.Random.Range(_lowPith, _topPith);

        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip);

        _audioSource.pitch = currentPitch;
    }

    private void OnSoundVolumeChanged() =>
        _audioSource.volume = _audioManager.SoundVolume;

    private void OnSoundMuteChanged() =>
        _audioSource.mute = _audioManager.IsSoundMute;
}
