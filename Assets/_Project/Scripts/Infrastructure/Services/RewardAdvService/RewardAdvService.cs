using System;
using YG;

public class RewardAdvService : IRewardAdvService, IDisposable
{
    public RewardAdvService() => 
        YG2.onRewardAdv += OnGiveReward;

    public event Action<RewardType> GiveReward;

    public void Dispose() => 
        YG2.onRewardAdv -= OnGiveReward;

    public void ShowRewardAdv(RewardType rewardID) => 
        YG2.RewardedAdvShow(rewardID.ToString());

    private void OnGiveReward(string rewardID)
    {
        if (Enum.TryParse(rewardID, out RewardType rewardType))        
            GiveReward?.Invoke(rewardType);        
        else       
            return;        
    }
}