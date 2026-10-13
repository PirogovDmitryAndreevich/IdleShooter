using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftRewardButton : MonoBehaviour
{
    [SerializeField] private GiftType _type;
    [Space]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _button;

    private IGiftService _giftService;
    private float _giftValue;
    private ITimeRewardManager _timeRewardManager;

    public bool IsAvailable { get; private set; }
    public GiftType Type => _type;

    private void Update()
    {
        if (_timeRewardManager != null && !IsAvailable)
            UpdateTimerText();
    }

    private void OnEnable()
    {
        if (_timeRewardManager != null)
            RefreshAvailability();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);

        if (_timeRewardManager != null)
            _timeRewardManager.AvailabilityChanged -= OnAvailabilityChanged;
    }

    public void Construct(IGiftService giftService, ITimeRewardManager timeRewardManager,
        GiftStaticData data, Sprite sprite)
    {
        _giftService = giftService;
        _icon.sprite = sprite;
        _timeRewardManager = timeRewardManager;

        _giftValue = data.Count;
        _countText.text = _giftValue.ToString();

        _button.onClick.AddListener(OnClick);

        _timeRewardManager.AvailabilityChanged += OnAvailabilityChanged;
    }

    private void OnClick()
    {
        if (!_timeRewardManager.TryClaim(_type))
            return;

        _giftService.AddGift(_type, _giftValue);
        RefreshAvailability();
    }

    private void RefreshAvailability()
    {
        IsAvailable = _timeRewardManager.IsAvailable(_type);
        _button.interactable = IsAvailable;

        UpdateTimerText();
    }

    private void OnAvailabilityChanged(GiftType type, bool isAvailable)
    {
        if (type != _type)
            return;

        RefreshAvailability();
    }

    private void UpdateTimerText()
    {
        if (_timerText == null)
            return;

        float remainingTime = _timeRewardManager.GetRemainingTime(_type);

        int totalSeconds = Mathf.CeilToInt(remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
}