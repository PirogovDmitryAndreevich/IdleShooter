using UnityEngine;

public class HudTutorialView : MonoBehaviour
{
    [SerializeField] private Sprite _player;
    [SerializeField] private Sprite _robot;
    [SerializeField] private GameObject _aimAnticlicker;
    [SerializeField] private GameObject _aimGlow;
    [SerializeField] private GameObject _anticlicker;
    [SerializeField] private Chat _chat;

    private IPauseService _pauseService;

    private void OnDestroy() => 
        _chat.ClickedButton -= OnHide;

    public void Construct(IPauseService pauseService)
    {
        _pauseService = pauseService;
        _aimAnticlicker.SetActive(false);
        _aimGlow.SetActive(false);
        _anticlicker.SetActive(false);
        _chat.Hide();

        _chat.ClickedButton += OnHide;
    }

    public void ShowAimTutorialMessage()
    {
        _aimAnticlicker.SetActive(true);
        SetAimGlow(true);
        _chat.Show(_player, MessageStringKey.Aim, BonusStringKey.AimBonus, false);

        _pauseService.Pause();
    }

    public void HideAimTutorialMessage()
    {
        _pauseService.Run();

        _aimAnticlicker.SetActive(false);
        _chat.Hide();
        SetAimGlow(false);
    }

    public void ShowLevelUpMessage()
    {
        _anticlicker.SetActive(true);
        _chat.Show(_player, MessageStringKey.LevelUp, BonusStringKey.LevelUpBonus);
        _pauseService.Pause();
    }

    public void ShowGiveWinsMessage()
    {
        _anticlicker.SetActive(true);
        _chat.Show(_player, MessageStringKey.Wins, BonusStringKey.WinsBonus);
        _pauseService.Pause();
    }

    public void ShowScoreMessage()
    {
        _anticlicker.SetActive(true);
        _chat.Show(_player, MessageStringKey.Score, BonusStringKey.ScoreBonus);
        _pauseService.Pause();
    }

    public void ShowCrownMessage()
    {
        _anticlicker.SetActive(true);
        _chat.Show(_player, MessageStringKey.Crowns, BonusStringKey.CrownsBonus);
        _pauseService.Pause();
    }

    private void OnHide()
    {
        _pauseService.Run();
        _anticlicker.SetActive(false);
    }

    private void SetAimGlow(bool isActive) =>
        _aimGlow.SetActive(isActive);
}
