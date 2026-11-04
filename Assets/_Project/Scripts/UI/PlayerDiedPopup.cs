using UnityEngine;
using UnityEngine.UI;

public class PlayerDiedPopup : MonoBehaviour
{
    [SerializeField] private Button _retryButton; 
    [SerializeField] private Button _returnButton;

    private WindowId _windowId = WindowId.PlayerDied;

    private SoundsHandler _soundsHandler;
    private SoundsData _soundsData;
    private IPlayerDiedService _playerDiedService;
    private IPauseService _pauseService;
    private IWindowsService _windowService;

    private void OnDestroy()
    {
        _retryButton.onClick.RemoveListener(OnRetryButtonClicked);
        _returnButton.onClick.RemoveListener(OnReturnButtonClicked);
    }

    public void Construct(IPlayerDiedService playerDiedService, IWindowsService windowsService,
        IPauseService pauseService, SoundsHandler soundsHandler, SoundsData soundsData)
    {
        _playerDiedService = playerDiedService;
        _pauseService = pauseService;
        _windowService = windowsService;
        _soundsHandler = soundsHandler;
        _soundsData = soundsData;

        _windowService.OpeningWindow += OnOpeningWindow;

        _retryButton.onClick.AddListener(OnRetryButtonClicked);
        _returnButton.onClick.AddListener(OnReturnButtonClicked);

        gameObject.SetActive(false);
    }

    private void OnOpeningWindow(WindowId id)
    {
        if (id != _windowId)
            return;

        gameObject.SetActive(true);
        _soundsHandler.PlaySound(_soundsData.OpenWindow);
        _pauseService.Pause();
    }

    private void Close()
    {
        _pauseService.Run();
        _soundsHandler.PlaySound(_soundsData.CloseWindow);
        gameObject.SetActive(false);
    }

    private void OnReturnButtonClicked()
    {
        _playerDiedService.OnUserNoShowedAdv();
        Close();
    }

    private void OnRetryButtonClicked()
    {
        _playerDiedService.OnUserShowedAdv();
        Close();
    }
}