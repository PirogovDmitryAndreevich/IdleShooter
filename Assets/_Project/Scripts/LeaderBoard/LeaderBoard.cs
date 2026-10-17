using UnityEngine;
using YG;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderboardYG _leaderboard;

    private ICrownsManager _crownsManager;

    private void OnDestroy() =>
       _crownsManager.CrownChanged -= SetNewValueLB;

    public void Construct(ICrownsManager crownsManager)
    {
        _crownsManager = crownsManager;
        _crownsManager.CrownChanged += SetNewValueLB;

        SetNewValueLB();
    }

    private void SetNewValueLB()
    {
        _leaderboard.SetLeaderboard(_crownsManager.GetCurrentCrowns());
        _leaderboard.UpdateLB();
    }
}
