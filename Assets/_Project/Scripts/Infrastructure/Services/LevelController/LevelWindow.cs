public class LevelWindow
{
    private readonly LevelRegistry _registry;

    public LevelWindow(LevelRegistry registry)
    {
        _registry = registry;
    }

    public void Update(LevelType current)
    {
        LevelType? previous = GetPreviousLevel(current);
        LevelType? next = GetNextLevel(current);

        foreach (LevelPresenter level in _registry.All)
        {
            bool shouldBeActive =
                level.LevelType == current ||
                level.LevelType == previous ||
                level.LevelType == next;

            level.SetActive(shouldBeActive);
        }
    }

    public LevelType? GetNextLevel(LevelType type)
    {
        return type switch
        {
            LevelType.Lobby => LevelType.Level_1,
            LevelType.Level_1 => LevelType.Level_2,
            LevelType.Level_2 => LevelType.Level_3,
            LevelType.Level_3 => LevelType.Level_4,

            _ => null
        };
    }

    public LevelType? GetPreviousLevel(LevelType type)
    {
        return type switch
        {
            LevelType.Level_1 => LevelType.Lobby,
            LevelType.Level_2 => LevelType.Level_1,
            LevelType.Level_3 => LevelType.Level_2,
            LevelType.Level_4 => LevelType.Level_3,

            _ => null
        };
    }
}