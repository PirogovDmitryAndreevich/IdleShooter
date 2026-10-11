using System;

public interface IRewardAdvService : IService
{
    void ShowRewardAdv(RewardType rewardID);

    event Action<RewardType> GiveReward;
}
