using DG.Tweening;
using UnityEngine;

public class WeaponWarningPopup : MonoBehaviour
{
    [SerializeField] private float _scaleMultiplier = 1.15f;
    [SerializeField] private float _duration = 0.5f;

    private Tween _scaleTween;
    private Vector3 _initialScale;

    private void Awake() => 
        _initialScale = transform.localScale;

    private void OnEnable() =>
    ImpulseView();

    private void OnDisable() =>
        StopImpulse();

    public void View(bool isView) =>
        gameObject.SetActive(isView);

    private void ImpulseView()
    {
        StopImpulse();

        transform.localScale = _initialScale;

        _scaleTween = transform
            .DOScale(_initialScale * _scaleMultiplier, _duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }

    private void StopImpulse()
    {
        _scaleTween?.Kill();
        _scaleTween = null;

        transform.localScale = _initialScale;
    }
}