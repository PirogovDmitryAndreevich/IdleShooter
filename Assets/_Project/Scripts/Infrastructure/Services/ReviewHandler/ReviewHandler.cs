using System;
using YG;

public class ReviewHandler : IReviewHandler
{
    private readonly ILevelManager _levelManager;
    private readonly GameStaticData _staticData;

    public ReviewHandler(ILevelManager levelManager, IStaticDataService staticDataService)
    {
        _levelManager = levelManager;
        _staticData = staticDataService.GetGameData();
    }

    public event Action ReviewAvailable;
    public event Action ReviewNotAvailable;

    public void ShowReviewButton() =>
        ReviewAvailable?.Invoke();

    public void HideReviewButton() =>
        ReviewNotAvailable?.Invoke();

    public void UpdateData()
    {
#if UNITY_EDITOR
        YG2.reviewCanShow = true;
#endif
        if (YG2.reviewCanShow)
            ShowReviewButton();
        else
            HideReviewButton();
    }

    public void OnShowReview()
    {
        if (YG2.reviewCanShow)        
            ShowReview();        
    }

    public void ShowReview()
    {
        YG2.reviewCanShow = false;
        YG2.onReviewSent += ReviewSent;
        YG2.ReviewShow();
    }

    private void ReviewSent(bool sent)
    {
        YG2.onReviewSent -= ReviewSent;

        if (sent)        
            OnReviewSuccess();        
        else        
            OnReviewFailed();        

        ReviewNotAvailable.Invoke();
    }

    private void OnReviewSuccess()
    {
        _levelManager.AddScore(100);
    }

    private void OnReviewFailed()
    {
        YG2.reviewCanShow = true;
    }
}