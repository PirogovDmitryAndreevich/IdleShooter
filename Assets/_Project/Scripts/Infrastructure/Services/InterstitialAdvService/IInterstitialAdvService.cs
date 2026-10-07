using System;

public interface IInterstitialAdvService : IService
{
    bool CanShow { get; }

    event Action ShowInterstitial;

    void TryShowAdv();
    void OnHideTimer();
    void ShowAdv();
}