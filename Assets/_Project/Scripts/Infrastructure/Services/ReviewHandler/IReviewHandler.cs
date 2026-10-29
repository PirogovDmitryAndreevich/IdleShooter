using System;

public interface IReviewHandler : IService
{
    event Action ReviewAvailable;
    event Action ReviewNotAvailable;

    void ShowReview();
    void UpdateData();
}