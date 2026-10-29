using UnityEngine;
using UnityEngine.UI;

public class ReviewPopup : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private WindowId _windowId;

    [Header("Review")]
    [SerializeField] private Button _showReview;

    private IWindowsService _windowService;
    private IPauseService _pause;
    private IReviewHandler _reviewHandler;

    private void OnDestroy()
    {
        _windowService.OpeningWindow -= OnOpenWindow;
        _closeButton.onClick.RemoveListener(Close);
        _showReview.onClick.RemoveListener(OnShowReview);
    }

    public void Construct(IWindowsService windowsService, IPauseService pause,
        IReviewHandler reviewHandler)
    {
        _windowService = windowsService;
        _pause = pause;
        _reviewHandler = reviewHandler;

        _showReview.onClick.AddListener(OnShowReview);
        _closeButton.onClick.AddListener(Close);
        _windowService.OpeningWindow += OnOpenWindow;

        Close();
    }

    private void OnShowReview()
    {
        _reviewHandler.ShowReview();
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