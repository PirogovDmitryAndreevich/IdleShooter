using DG.Tweening;
using UnityEngine;

public class CameraShake
{
    private readonly Transform _shake;

    private Tween _positionTween;
    private Tween _rotationTween;

    private Quaternion _rotationCashed;
    private Vector3 _positionCashed;
    private float _rotationSpeed = 8f;
    private float _positionSpeed;

    public CameraShake(Transform shake)
    {
        _shake = shake;

        _rotationCashed = _shake.localRotation;
        _positionCashed = _shake.localPosition;
    }

    public void LateUpdate(float deltaTime)
    {
        var rotation = _shake.localRotation;
        var position = _shake.localPosition;

        rotation = Quaternion.Lerp(rotation, _rotationCashed, deltaTime * _rotationSpeed);
        position = Vector3.Lerp(position, _positionCashed, deltaTime * _positionSpeed);

        _shake.localRotation = rotation;
        _shake.localPosition = position;
    }

    public void Shake(ShootShakeSettings settings)
    {
        _positionTween = _shake.DOShakePosition(
            settings.PositionShakeSettings.Duration,
            settings.PositionShakeSettings.Strength,
            settings.PositionShakeSettings.Vibrato,
            settings.PositionShakeSettings.Randomness,
            false,
            settings.PositionShakeSettings.FadeOut)
            .SetEase(Ease.InOutBounce)
            .SetLink(_shake.gameObject)
            .Play();

        _rotationTween = _shake.DOShakeRotation(
            settings.RotationShakeSettings.Duration,
            settings.RotationShakeSettings.Strength,
            settings.RotationShakeSettings.Vibrato,
            settings.RotationShakeSettings.Randomness,
            settings.RotationShakeSettings.FadeOut)
            .SetEase(Ease.InOutBounce)
            .SetLink(_shake.gameObject)
            .Play();
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