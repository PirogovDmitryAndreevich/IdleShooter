using System;
using UnityEngine;

public class SoundsHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Space]
    [SerializeField] private float _lowPith = 0f;
    [SerializeField] private float _topPith = 2f;

    private ISettingsManager _audioManager;

    private void OnDestroy()
    {
        if (_audioManager == null)
            return;

        _audioManager.SoundVolumeChanged -= OnSoundVolumeChanged;
        _audioManager.SoundMuteChanged -= OnSoundMuteChanged;
    }

    public void Construct(ISettingsManager audioManager)
    {
        _audioManager = audioManager;

        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        OnSoundVolumeChanged();
        OnSoundMuteChanged();

        _audioManager.SoundVolumeChanged += OnSoundVolumeChanged;
        _audioManager.SoundMuteChanged += OnSoundMuteChanged;
    }

    private float _nextPlayStepTime;

    public void PlayStepSound(AudioClip clip)
    {
        if (_nextPlayStepTime < Time.time)
        {
            PlayRandomPithSound(clip);
            _nextPlayStepTime = clip.length + Time.time;
        }
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

    public void StopSound() =>
        _audioSource.Stop();

    private void OnSoundVolumeChanged() =>
        _audioSource.volume = _audioManager.SoundVolume;

    private void OnSoundMuteChanged() =>
        _audioSource.mute = _audioManager.IsSoundMute;
}
