using UnityEngine;

public class Gift : MonoBehaviour
{
    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    private WindowId _windowId = WindowId.Gifts;

    private IWindowsService _windowService;

    private void OnDestroy() => 
        _triggerObserver.PlayerEntered -= OnPlayerEntered;

    public void Construct(IWindowsService windowsService)
    {
        _windowService = windowsService;

        _triggerObserver.PlayerEntered += OnPlayerEntered;
    }

    private void OnPlayerEntered(IPlayer player) => 
        _windowService.OpenWindow(_windowId);
}