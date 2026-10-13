using UnityEngine;

public class Gift : MonoBehaviour
{
    [SerializeField] private PlayerTriggerObserver _triggerObserver;
    [SerializeField] private GameObject _onHasAvailableGifts;

    private WindowId _windowId = WindowId.Gifts;

    private IWindowsService _windowService;
    private ITimeRewardManager _timeRewardManager;

    private void OnDestroy() => 
        _triggerObserver.PlayerEntered -= OnPlayerEntered;

    public void Construct(IWindowsService windowsService, ITimeRewardManager timeRewardManager)
    {
        _windowService = windowsService;
        _timeRewardManager = timeRewardManager;

        _triggerObserver.PlayerEntered += OnPlayerEntered;

        _timeRewardManager.AvailabilityChanged += OnAvailabilityChanged;

        UpdateView();
    }

    private void OnAvailabilityChanged(GiftType type, bool isAvailable) =>
        UpdateView();

    private void OnPlayerEntered(IPlayer player) => 
        _windowService.OpenWindow(_windowId);

    private void UpdateView()
    {
        if (_onHasAvailableGifts != null && _timeRewardManager != null)
            _onHasAvailableGifts.SetActive(_timeRewardManager.HasAvailableGifts());
    }
}