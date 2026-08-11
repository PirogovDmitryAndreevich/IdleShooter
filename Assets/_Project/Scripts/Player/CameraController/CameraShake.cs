using DG.Tweening;
using UnityEngine;

public class CameraShake
{
    private readonly Transform _camera;

    private readonly Vector3 _defaultLocalPosition;
    private readonly Quaternion _defaultLocalRotation;

    private Tween _positionTween;
    private Tween _rotationTween;

    public CameraShake(Transform camera)
    {
        _camera = camera;

        _defaultLocalPosition = camera.localPosition;
        _defaultLocalRotation = Quaternion.Euler(30f, 0f, 0f);

        _camera.localRotation = _defaultLocalRotation;
    }

    public void Shake(
        float duration,
        float positionStrength,
        float rotationStrength,
        int vibrato = 20,
        float randomness = 90f)
    {
        Stop();

        _camera.localPosition = _defaultLocalPosition;
        _camera.localRotation = _defaultLocalRotation;

        _positionTween = _camera.DOShakePosition(
            duration,
            positionStrength,
            vibrato,
            randomness,
            false,
            true).Play();

        _rotationTween = _camera.DOShakeRotation(
            duration,
            rotationStrength,
            vibrato,
            randomness,
            true).Play();
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
        _camera.localPosition = _defaultLocalPosition;
        _camera.localRotation = _defaultLocalRotation; 
    }
}