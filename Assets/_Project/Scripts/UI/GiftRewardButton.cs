using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftRewardButton : MonoBehaviour
{
    [SerializeField] private GiftType _type;
    [Space]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _bgFrame;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _collectFrame;
    [SerializeField] private Color _noAvailable;
    [SerializeField] private Color _available;

    private IGiftService _giftService;
    private float _giftValue;
    private ITimeRewardManager _timeRewardManager;
    private SoundsHandler _soundsHandler;
    private SoundsData _soundsData;

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
        GiftStaticData data, Sprite sprite, SoundsHandler soundsHandler, SoundsData soundsData)
    {
        _bgFrame.color = _noAvailable;

        _giftService = giftService;
        _icon.sprite = sprite;
        _timeRewardManager = timeRewardManager;
        _soundsHandler = soundsHandler;
        _soundsData = soundsData;

        _giftValue = data.Count;

        if (_type == GiftType.fourth || _type == GiftType.third)
            _countText.text = "X" + _giftValue.ToString();
        else
            _countText.text = _giftValue.ToString();

        _button.onClick.AddListener(OnClick);

        _timeRewardManager.AvailabilityChanged += OnAvailabilityChanged;

        _collectFrame.gameObject.SetActive(false);
    }

    private void OnClick()
    {
        if (!_timeRewardManager.TryClaim(_type))
            return;

        _soundsHandler.PlaySound(_soundsData.GiftButtonCollect);
        _collectFrame.SetActive(true);
        _bgFrame.color = _noAvailable;
        _giftService.AddGift(_type, _giftValue);
        RefreshAvailability();
    }

    private void RefreshAvailability()
    {
        IsAvailable = _timeRewardManager.IsAvailable(_type);
        _button.interactable = IsAvailable;
        _bgFrame.color = IsAvailable ? _available : _noAvailable;

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