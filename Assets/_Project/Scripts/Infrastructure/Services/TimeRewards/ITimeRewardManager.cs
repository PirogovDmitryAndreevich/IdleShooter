using System;

public interface ITimeRewardManager : IService
{
    event Action<GiftType, bool> AvailabilityChanged;

    void Update();
    void RegisterGift(GiftType type, float durationMinutes);
    bool IsAvailable(GiftType type);
    float GetRemainingTime(GiftType type);
    bool TryClaim(GiftType type);
    bool HasAvailableGifts();
}