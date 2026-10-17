public class TutorialService : ITutorialService
{
    public bool IsReady =>
        Player != null && StonesSwitcher != null
        && PassiveShooting != null && Hud != null
        && LevelView_1 != null;

    public PlayerPresenter Player { get; private set; }
    public SwitchWeaponButton StonesSwitcher { get; private set; }
    public PassiveShooting PassiveShooting { get; private set; }
    public Hud Hud { get; private set; }
    public LevelView_1 LevelView_1 { get; private set; }
    public Transition Transition2 {  get; private set; }

    public void SetPlayerPresenter(PlayerPresenter playerPresenter) =>
        Player = playerPresenter;

    public void SetStonesWeaponSwitcher(SwitchWeaponButton weaponSwitcher) =>
        StonesSwitcher = weaponSwitcher;

    public void SetPassiveShooter(PassiveShooting passiveShooting) =>
        PassiveShooting = passiveShooting;

    public void SetHud(Hud hud) =>
        Hud = hud;

    public void SetLevelView_1(LevelView_1 level) =>
        LevelView_1 = level;

    public void SetTransition2(Transition transition) =>
        Transition2 = transition;
}