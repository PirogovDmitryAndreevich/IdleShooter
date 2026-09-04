public class LevelWindow
{
    private readonly LevelRegister _register;
    private readonly LevelSequence _sequence;

    public LevelWindow(LevelRegister register, LevelSequence sequence)
    {
        _register = register;
        _sequence = sequence;
    }

    public void Update(LevelType current)
    {
        LevelType? previous =
            _sequence.GetPrevious(current);

        LevelType? next =
            _sequence.GetNext(current);

        foreach (LevelPresenter level in _register.All)
        {
            bool shouldBeActive =
                level.LevelType == current ||
                level.LevelType == previous ||
                level.LevelType == next;

            level.SetActive(shouldBeActive);
        }
    }
}