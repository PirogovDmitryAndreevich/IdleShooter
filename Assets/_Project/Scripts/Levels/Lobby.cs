public class Lobby : LevelView
{
    public override LevelType LevelType => LevelType.Lobby;

    public WeaponSwitcherSpawnPoint[] WeaponSwitcherSpawnPoint;
    public WarpSpawnPoint[] WarpSpawnPoint;
    public PlayerSpawnPoint PlayerSpawnPoint;
}
