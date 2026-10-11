using UnityEngine;
using UnityEngine.UI;

public class WindowBase : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private WindowId _windowId;

    private IWindowsService _windowService;


    private void OnDestroy()
    {
        _windowService.OpeningWindow -= OnOpenWindow;
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Construct(IWindowsService windowsService)
    {
        _windowService = windowsService;

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
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}