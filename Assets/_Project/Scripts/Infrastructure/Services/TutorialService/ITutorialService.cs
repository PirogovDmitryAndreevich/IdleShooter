public interface ITutorialService : IService
{
    bool IsReady { get; }
    Hud Hud { get; }
    PassiveShooting PassiveShooting { get; }
    PlayerPresenter Player { get; }
    SwitchWeaponButton StonesSwitcher { get; }
    LevelView_1 LevelView_1 { get; }
    Transition Transition2 {  get; }

    void SetHud(Hud hud);
    void SetPassiveShooter(PassiveShooting passiveShooting);
    void SetPlayerPresenter(PlayerPresenter playerPresenter);
    void SetStonesWeaponSwitcher(SwitchWeaponButton weaponSwitcher);
    void SetLevelView_1(LevelView_1 level);
    void SetTransition2(Transition transition);
}