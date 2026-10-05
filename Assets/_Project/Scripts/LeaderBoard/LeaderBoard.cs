using UnityEngine;
using YG;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderboardYG _leaderboard;

    private ILevelManager _levelManager;

    private void OnDestroy() => 
        _levelManager.LevelChanged -= SetNewValueLB;

    public void Construct(ILevelManager levelManager)
    {
        _levelManager = levelManager;
        _levelManager.LevelChanged += SetNewValueLB;

        SetNewValueLB(_levelManager.CurrentLevel);
    }

    private void SetNewValueLB(int level)
    {
        _leaderboard.SetLeaderboard(_levelManager.CurrentLevel);
        _leaderboard.UpdateLB();
    }
}
