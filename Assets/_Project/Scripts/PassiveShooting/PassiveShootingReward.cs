using TMPro;
using UnityEngine;

public class PassiveShootingReward : PassiveShooting
{
    [Header("Timers")]
    [SerializeField] private TMP_Text _accessTimerText;
    [SerializeField] private TMP_Text _rewardIntervalText;

    [Header("Settings")]
    [SerializeField] private float _showRewardInterval = 10f;
    [SerializeField] private float _accessTime = 30f;

    private RewardType _rewardType = RewardType.PassiveShooting;

    private float _rewardInterval;
    private float _timer;

    private IRewardAdvService _rewardService;

    private void Update()
    {
        if (IsAccess)
        {
            _timer -= Time.deltaTime;

            UpdateTimerText(_accessTimerText, _timer);

            if (_timer <= 0f)
                CloseAccess();

            return;
        }

        if (_rewardInterval > 0f)
        {
            _rewardInterval -= Time.deltaTime;

            UpdateTimerText(_rewardIntervalText, _rewardInterval);

            if (_rewardInterval <= 0f)
                HideTimer(_rewardIntervalText);
        }
    }

    public void Construct(IRewardAdvService rewardService, IProgressService progress)
    {
        _rewardService = rewardService;
        _rewardService.GiveReward += OnGiveReward;

        IsAccess = false;
        _timer = 0f;
        _rewardInterval = 0f;

        HideTimer(_accessTimerText);
        HideTimer(_rewardIntervalText);
    }

    protected override void Destroy()
    {
        base.Destroy();

        _rewardService.GiveReward -= OnGiveReward;
    }

    private void OnGiveReward(RewardType type)
    {
        if (_rewardType != type)
            return;

        OpenAccess();
    }

    protected override void OnPlayerEntered(IPlayer player)
    {
        if (player == null)
            return;

        Player = player;
        IsPlayerInside = true;

        if (IsAccess)
        {
            base.OnPlayerEntered(player);
            return;
        }

        if (_rewardInterval > 0f)
            return;

        _rewardService.ShowRewardAdv(_rewardType);
    }

    private void OpenAccess()
    {
        IsAccess = true;
        _timer = _accessTime;
        _rewardInterval = 0f;

        HideTimer(_rewardIntervalText);

        if (_timer > 0f)
        {
            ShowTimer(_accessTimerText);
            UpdateTimerText(_accessTimerText, _timer);
        }
        else
        {
            CloseAccess();
            return;
        }

        if (IsPlayerInside && Player != null)
            OnPlayerEntered(Player);
    }

    private void CloseAccess()
    {
        if (!IsAccess)
            return;

        if (IsPlayerInside && Player != null)
            Player.OnStopPassiveShooting();

        IsAccess = false;
        _timer = 0f;

        HideTimer(_accessTimerText);

        _rewardInterval = _showRewardInterval;

        if (_rewardInterval > 0f)
        {
            ShowTimer(_rewardIntervalText);
            UpdateTimerText(_rewardIntervalText, _rewardInterval);
        }
        else
        {
            HideTimer(_rewardIntervalText);
        }
    }

    private void UpdateTimerText(TMP_Text timerText, float time)
    {
        if (timerText == null)
            return;

        int totalSeconds = Mathf.CeilToInt(Mathf.Max(0f, time));
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void ShowTimer(TMP_Text timerText)
    {
        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }

    private void HideTimer(TMP_Text timerText)
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }
}