using DG.Tweening;
using UnityEngine;

public class ShootEffectUIAnimation
{
    private readonly RectTransform _aim;
    private readonly ShootUIEffectAnimationSettings _settings;

    public ShootEffectUIAnimation(RectTransform aim, ShootUIEffectAnimationSettings settings)
    {
        _aim = aim;
        _settings = settings;
    }

    public void StartAnimation(ShootUIEffect effect)
    {
        RectTransform effectTransform = effect.RectTransform;
        CanvasGroup canvasGroup = effect.CanvasGroup;

        effectTransform.position = _aim.position;
        effectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        Vector2 direction = GetRandomDirection();

        Vector3 startPosition = effectTransform.position;

        Vector3 targetPosition = startPosition + (Vector3)(direction * _settings.MoveDistance);

        Sequence sequence = DOTween.Sequence();

        // Ѕыстрый вылет вверх.
        sequence.Append( effectTransform .DOMove( startPosition + (targetPosition - startPosition) * 0.65f,
            _settings.MoveDuration * 0.55f).SetEase(Ease.OutQuad));

        // ћ€гкое продолжение движени€.
        sequence.Append(effectTransform.DOMove(targetPosition, _settings.AnimationDuration * 0.45f).SetEase(Ease.OutSine));       
        sequence.Join(canvasGroup.DOFade(0f, _settings.AnimationDuration).SetEase(Ease.InQuad));

        sequence.Play();
        sequence.OnComplete(effect.Hide);
    }

    private Vector2 GetRandomDirection()
    {
        float angle = Random.Range(-_settings.MaxHorizontalAngle, _settings.MaxHorizontalAngle);

        float radians = angle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
    }
}