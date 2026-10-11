public class Lobby : LevelView
{
    public override LevelType LevelType => LevelType.Lobby;

    public LeaderBoard LeaderBoard;
    public WeaponSwitcherSpawnPoint[] WeaponSwitcherSpawnPoint;
    public WarpSpawnPoint[] WarpSpawnPoint;
    public PlayerSpawnPoint PlayerSpawnPoint;
    public Transition LobbyTransition;
    public Transition TransitionFor1Level;
    public PassiveShootingReward PassiveShootingReward;
    public Gift Gift;
}
