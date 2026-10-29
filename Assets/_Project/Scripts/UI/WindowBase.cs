using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowBase : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private WindowId _windowId;

    private SoundsHandler _soundsHandler;
    private SoundsData _soundsData;
    private IWindowsService _windowService;
    private IPauseService _pause;

    private void OnDestroy()
    {
        _windowService.OpeningWindow -= OnOpenWindow;
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Construct(IWindowsService windowsService, IPauseService pause,
         SoundsHandler soundsHandler, SoundsData soundsData)
    {
        _windowService = windowsService;
        _pause = pause;
        _soundsHandler = soundsHandler;
        _soundsData = soundsData;

        _closeButton.onClick.AddListener(Close);
        _windowService.OpeningWindow += OnOpenWindow;

        gameObject.SetActive(false);
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
        _soundsHandler.PlaySound(_soundsData.OpenWindow);
        _pause.Pause();
    }

    private void Close()
    {
        _pause.Run();
        _soundsHandler.PlaySound(_soundsData.CloseWindow);
        gameObject.SetActive(false);
    }
}
