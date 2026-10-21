using UnityEngine;
using UnityEngine.UI;

public class WindowBase : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private WindowId _windowId;

    private IWindowsService _windowService;
    private IPauseService _pause;

    private void OnDestroy()
    {
        _windowService.OpeningWindow -= OnOpenWindow;
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Construct(IWindowsService windowsService, IPauseService pause)
    {
        _windowService = windowsService;
        _pause = pause;

        _closeButton.onClick.AddListener(Close);
        _windowService.OpeningWindow += OnOpenWindow;

        Close();
    }

    private void OnOpenWindow(WindowId id)
    {
        if (id != _windowId)
            return;

        Open();        
    }

    private void Open()
    {
        gameObject.SetActive(true);
        _pause.Pause();
    }

    private void Close()
    {
        _pause.Run();
        gameObject.SetActive(false);
    }
}
