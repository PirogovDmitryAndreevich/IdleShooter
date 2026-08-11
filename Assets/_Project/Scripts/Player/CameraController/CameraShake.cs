using DG.Tweening;
using UnityEngine;

public class CameraShake
{
    private readonly Transform _shake;

    private Tween _positionTween;
    private Tween _rotationTween;

    public CameraShake(Transform shake)
    {
        _shake = shake;
    }

    public void Shake(ShootShakeSettings settings)
    {
        Stop();

        _shake.localPosition = Vector3.zero;
        _shake.localRotation = Quaternion.identity;

        _positionTween = _shake.DOShakePosition(
            settings.Duration,
            settings.PositionStrength,
            settings.Vibrato,
            settings.Randomness,
            false,
            settings.FadeOut).Play();

        _rotationTween = _shake.DOShakeRotation(
            settings.Duration,
            settings.RotationStrength,
            settings.Vibrato,
            settings.Randomness,
            settings.FadeOut).Play();
    }

    public void Stop()
    {
        _positionTween?.Kill();
        _rotationTween?.Kill();

        _positionTween = null;
        _rotationTween = null;

        Reset();
    }

    private void Reset()
    {
        _shake.localPosition = Vector3.zero;
        _shake.localRotation = Quaternion.identity;
    }
}