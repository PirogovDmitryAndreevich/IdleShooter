using UnityEngine;
using UnityEngine.UI;

public class WindowButton : MonoBehaviour
{
    [SerializeField] private WindowId _windowId;
    [SerializeField] private Button _button;

    private IWindowsService _windowService;

    private void OnDestroy() => 
        _button.onClick.RemoveListener(OnClick);

    public void Construct(IWindowsService windowsService)
    {
        _windowService = windowsService;

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick() => 
        _windowService.OpenWindow(_windowId);
}
