using DG.Tweening;
using UnityEngine;

public class ImpactPunchScale
{
    private const float Impact = 0.3f;
    private const float Duration = 0.5f;
    private const int Vibrato = 1;
    private const float Elasticity = 0.5f;

    private readonly Transform _target;
    private readonly Vector3 _startScale;

    private Tween _impactTween;

    public ImpactPunchScale(Transform target)
    {
        _target = target;
        _startScale = target.localScale;

        _impactTween = _target
            .DOPunchScale(
                new Vector3(
                    Impact,
                    -Impact,
                    Impact * 0.5f),
                Duration,
                Vibrato,
                Elasticity)
            .SetEase(Ease.OutQuad)
            .SetLink(target.gameObject)
            .SetAutoKill(false);
    }

    public void Play()
    {
        if (_impactTween.IsPlaying())
        {
            return;
        }

        _impactTween.Restart();
    }

    public void Reset()
    {
        _target.localScale = _startScale;
    }
}