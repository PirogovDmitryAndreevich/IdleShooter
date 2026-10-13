using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewardManager : IDisposable, ITimeRewardManager
{
    private const float MinuteCoefficient = 60f;

    private readonly Dictionary<GiftType, GiftTimerData> _gifts = new();

    private bool _isDisposed;

    public event Action<GiftType, bool> AvailabilityChanged;

    public void Update()
    {
        if (_isDisposed)
            return;

        foreach (KeyValuePair<GiftType, GiftTimerData> pair in _gifts)
        {
            GiftTimerData gift = pair.Value;

            if (gift.IsClaimed || gift.IsAvailable)
                continue;

            if (Time.unscaledTime < gift.EndTime)
                continue;

            gift.IsAvailable = true;
            AvailabilityChanged?.Invoke(pair.Key, true);
        }
    }

    public void RegisterGift(GiftType type, float durationMinutes)
    {
        if (_isDisposed || _gifts.ContainsKey(type))
            return;

        float duration = durationMinutes * MinuteCoefficient;

        _gifts.Add(type, new GiftTimerData(
            Time.unscaledTime + duration
        ));
    }

    public bool IsAvailable(GiftType type)
    {
        return _gifts.TryGetValue(type, out GiftTimerData gift)
            && gift.IsAvailable
            && !gift.IsClaimed;
    }

    public float GetRemainingTime(GiftType type)
    {
        if (!_gifts.TryGetValue(type, out GiftTimerData gift))
            return 0f;

        if (gift.IsAvailable || gift.IsClaimed)
            return 0f;

        return Mathf.Max(0f, gift.EndTime - Time.unscaledTime);
    }

    public bool TryClaim(GiftType type)
    {
        if (!IsAvailable(type))
            return false;

        GiftTimerData gift = _gifts[type];

        gift.IsClaimed = true;
        gift.IsAvailable = false;

        AvailabilityChanged?.Invoke(type, false);

        return true;
    }

    public bool HasAvailableGifts()
    {
        foreach (GiftTimerData gift in _gifts.Values)
        {
            if (gift.IsAvailable && !gift.IsClaimed)
                return true;
        }

        return false;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        AvailabilityChanged = null;
        _gifts.Clear();
    }    

    private class GiftTimerData
    {
        public float EndTime { get; }
        public bool IsAvailable { get; set; }
        public bool IsClaimed { get; set; }

        public GiftTimerData(float endTime)
        {
            EndTime = endTime;
        }
    }
}