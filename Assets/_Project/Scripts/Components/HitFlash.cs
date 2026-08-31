using DG.Tweening;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    private static readonly int HitEffectId =
        Shader.PropertyToID("_HitEffect");

    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private float _duration = 0.12f;

    private MaterialPropertyBlock _propertyBlock;

    private Tween _tween;
    private float _value;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();

        SetHitEffect(0f);

        _value = 1f;

        _tween = DOTween.To(
            () => _value,
            value =>
            {
                _value = value; SetHitEffect(value);
            },
            0f,
            _duration)
            .SetAutoKill(false)
            .SetEase(Ease.OutQuad);
    }

    private void OnDestroy() =>
        _tween?.Kill();

    public void Flash()
    {
        _value = 1f;

        SetHitEffect(_value);

        _tween.Restart();
    }

    private void SetHitEffect(float value)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(HitEffectId, value);
            renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}