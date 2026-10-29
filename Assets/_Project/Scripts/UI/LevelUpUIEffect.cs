using DG.Tweening;
using UnityEngine;

public class LevelUpUIEffect : MonoBehaviour
{
    [SerializeField] private RectTransform _levelText;
    [SerializeField] private CanvasGroup _levelCanvasGroup;
    [SerializeField] private RectTransform[] _hearts;

    [Header("Level Text")]
    [SerializeField] private float _textMoveDistance = 70f;
    [SerializeField] private float _textShowDuration = 0.35f;
    [SerializeField] private float _textFadeDuration = 0.18f;

    [Header("Hearts")]
    [SerializeField] private float _heartScatterDuration = 0.3f;
    [SerializeField] private float _heartFlyDuration = 0.45f;

    [Header("Heart Scatter Points")]
    [SerializeField]
    private Vector2[] _heartScatterPoints =
        {
            new Vector2(-65f, -55f),
            new Vector2(-20f, -80f),
            new Vector2(25f, -75f),
            new Vector2(65f, -50f)
        };

    [Header("Heart Fly")]
    [SerializeField] private float _heartArcHorizontal = 40f;
    [SerializeField] private float _heartArcVertical = 25f;

    private RectTransform _healthBar;
    private Vector2 _levelTextStartPosition;
    private ILevelManager _levelManager;
    private SoundsHandler _soundsHandler;
    private SoundsData _soundsData;
    private Sequence _sequence;

    public void Construct(RectTransform healthBar, ILevelManager levelManager,
        SoundsHandler soundsHandler, SoundsData soundsData)
    {
        _healthBar = healthBar;
        _levelTextStartPosition = _levelText.anchoredPosition;
        _levelManager = levelManager;
        _soundsHandler = soundsHandler;
        _soundsData = soundsData;
    }

    public void Show()
    {
        KillAnimations();

        _soundsHandler.StopSound();

        _soundsHandler.PlaySound(_soundsData.LevelUp);

        Prepare();

        _sequence = DOTween.Sequence();

        Vector2 textTargetPosition = _levelTextStartPosition + Vector2.up * _textMoveDistance;

        // LevelText появляется и поднимается вверх.
        _sequence.Append(_levelText.DOAnchorPos(textTargetPosition, _textShowDuration).SetEase(Ease.OutBack));
        _sequence.Join(_levelCanvasGroup.DOFade(1f, _textShowDuration * 0.7f).SetEase(Ease.OutQuad));

        // 2. Только после завершения движения текста
        // включаем сердца.
        _sequence.AppendCallback(EnableHearts);

        // Сердца вылетают из текста.
        _sequence.Append(CreateHeartScatterSequence());

        // Level Text быстро исчезает.
        _sequence.Append(_levelCanvasGroup.DOFade(0f, _textFadeDuration).SetEase(Ease.InQuad));

        // Сердца летят в HealthBar.
        _sequence.Append(CreateHeartsFlySequence());

        _sequence.OnComplete(() =>
        {
            _sequence = null;

            _levelManager.UpdateHealthView();
            Hide();
        });

        _sequence.Play();
    }

    private void EnableHearts()
    { 
        Vector2 startPosition = _levelText.anchoredPosition;

        for (int i = 0; i < _hearts.Length; i++)
        {
            RectTransform heart = _hearts[i];

            heart.gameObject.SetActive(true);

            heart.anchoredPosition = startPosition;

            heart.localScale = Vector3.one;
        }
    }

    private Sequence CreateHeartScatterSequence()
    {
        Sequence sequence = DOTween.Sequence();
        Vector2 startPosition = _levelText.anchoredPosition;

        for (int i = 0; i < _hearts.Length; i++)
        {
            RectTransform heart = _hearts[i];

            heart.anchoredPosition = startPosition;
            heart.localScale = Vector3.one;

            Vector2 scatterOffset = GetScatterPosition(i);

            Vector2 targetPosition = startPosition + scatterOffset;

            Sequence heartSequence = DOTween.Sequence();

            heartSequence.Append(heart.DOAnchorPos(targetPosition, _heartScatterDuration).SetEase(Ease.OutQuad));

            // Небольшой разброс скорости появления.
            sequence.Join(heartSequence.SetDelay(i * 0.1f));
        }

        return sequence;
    }

    private Vector2 GetScatterPosition(int index)
    {
        if (index < _heartScatterPoints.Length)
            return _heartScatterPoints[index];

        // Если сердец случайно больше 4,
        // для дополнительных создаём запасную позицию.
        return new Vector2(Random.Range(-60f, 60f), Random.Range(-80f, -45f));
    }

    private Sequence CreateHeartsFlySequence()
    {
        Sequence sequence = DOTween.Sequence();

        Vector3 targetPosition = _healthBar.position;

        for (int i = 0; i < _hearts.Length; i++)
        {
            RectTransform heart = _hearts[i];

            Vector2 startPosition = heart.position;
            Vector3 middlePosition = Vector2.Lerp(startPosition, targetPosition, 0.5f);

            // Небольшая случайная дуга.
            middlePosition += new Vector3(GetArcOffset(i), Random.Range(-_heartArcVertical, _heartArcVertical), 0f);

            Sequence heartSequence = DOTween.Sequence();

            heartSequence.Append(heart.DOMove(middlePosition, _heartFlyDuration * 0.45f).SetEase(Ease.OutQuad));
            heartSequence.Append(heart.DOMove(targetPosition, _heartFlyDuration * 0.55f).SetEase(Ease.InQuad));

            // Небольшой эффект приближения к HealthBar.
            heartSequence.Insert(_heartFlyDuration * 0.65f, heart.DOScale(0.65f, _heartFlyDuration * 0.35f).SetEase(Ease.InQuad)); sequence.Join(heartSequence);
        }
        return sequence;
    }

    private float GetArcOffset(int index)
    {
        switch (index)
        {
            case 0:
                return -_heartArcHorizontal;
            case 1:
                return -_heartArcHorizontal * 0.35f;
            case 2:
                return _heartArcHorizontal * 0.35f;
            case 3:
                return _heartArcHorizontal;
            default:
                return Random.Range(-_heartArcHorizontal, _heartArcHorizontal);
        }
    }

    private void Prepare()
    {
        _levelText.gameObject.SetActive(true);

        _levelText.anchoredPosition = _levelTextStartPosition;
        _levelCanvasGroup.alpha = 0f;
        _levelText.localScale = Vector3.one;

        foreach (RectTransform heart in _hearts)
        {
            heart.DOKill();
            heart.gameObject.SetActive(false);
            heart.anchoredPosition = _levelTextStartPosition;
            heart.localScale = Vector3.one;
        }
    }

    private void Hide()
    {
        _levelText.gameObject.SetActive(false);

        _levelText.anchoredPosition = _levelTextStartPosition;

        foreach (RectTransform heart in _hearts)
        {
            heart.gameObject.SetActive(false);

            heart.anchoredPosition = _levelTextStartPosition;
            heart.localScale = Vector3.one;
        }
    }

    private void KillAnimations()
    {
        _sequence?.Kill();
        _sequence = null;

        _levelText.DOKill();

        foreach (RectTransform heart in _hearts)
            heart.DOKill();

        _levelCanvasGroup?.DOKill();
    }
}