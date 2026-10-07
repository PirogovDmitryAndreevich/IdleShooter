using System;
using UnityEngine;

public class InterstitialAdvService : IInterstitialAdvService
{
    private const float TimerForInterstitial = 60f;

    public InterstitialAdvService()
    {
        _isShowing = false;
        _timer = TimerForInterstitial;
    }

    private float _timer;
    private bool _isShowing;

    public bool CanShow =>
        _timer <= Time.time
        && !_isShowing;

    public event Action ShowInterstitial;

    public void TryShowAdv()
    {
        if (CanShow)        
            ShowAdv();        
    }

    public void ShowAdv()
    {
        _isShowing = true;
        ShowInterstitial?.Invoke();
    }

    public void OnHideTimer()
    {
        _isShowing = false;
        _timer = Time.time + TimerForInterstitial;
    }
}