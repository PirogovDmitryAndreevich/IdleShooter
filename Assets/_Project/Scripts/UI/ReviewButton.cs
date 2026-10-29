using UnityEngine;
using UnityEngine.UI;

public class ReviewButton : MonoBehaviour
{
    [SerializeField] private WindowId _windowId;
    [SerializeField] private Button _button;

    private IWindowsService _windowService;
    private IReviewHandler _reviewHandler;

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);

        _reviewHandler.ReviewAvailable -= OnAvailable;
        _reviewHandler.ReviewNotAvailable -= OnNotAvailable;
    }

    public void Construct(IWindowsService windowsService, IReviewHandler reviewHandler)
    {
        _windowService = windowsService;
        _reviewHandler = reviewHandler;

        _reviewHandler.ReviewAvailable += OnAvailable;
        _reviewHandler.ReviewNotAvailable += OnNotAvailable;
        _button.onClick.AddListener(OnClick);

        OnNotAvailable();
    }

    private void OnAvailable() => 
        gameObject.SetActive(true);

    private void OnNotAvailable() =>
        gameObject.SetActive(false);

    private void OnClick() =>
        _windowService.OpenWindow(_windowId);
}

