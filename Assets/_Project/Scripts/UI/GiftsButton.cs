using UnityEngine;
using UnityEngine.UI;

public class GiftsButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _marker;

    private IWindowsService _windowService;
    private ITimeRewardManager _timeRewardManager;

    private WindowId _windowId = WindowId.Gifts;

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);

        if (_timeRewardManager != null)
            _timeRewardManager.AvailabilityChanged -= OnAvailabilityChanged;
    }

    public void Construct(IWindowsService windowsService,
        ITimeRewardManager timeRewardManager)
    {
        _windowService = windowsService;
        _timeRewardManager = timeRewardManager;

        _button.onClick.AddListener(OnClick);

        _timeRewardManager.AvailabilityChanged += OnAvailabilityChanged;
        UpdateMarker();
    }

    private void OnClick() =>
        _windowService.OpenWindow(_windowId);

    private void OnAvailabilityChanged(GiftType type, bool isAvailable) => 
        UpdateMarker();

    private void UpdateMarker()
    {
        if (_marker != null && _timeRewardManager != null)
            _marker.SetActive(_timeRewardManager.HasAvailableGifts());
    }
}