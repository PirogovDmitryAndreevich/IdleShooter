using System;

public class LevelSequence
{
    private readonly LevelType[] _levels =
        (LevelType[])Enum.GetValues(typeof(LevelType));

    public LevelType? GetPrevious(LevelType type)
    {
        int index = Array.IndexOf(_levels, type);

        return index > 0
            ? _levels[index - 1]
            : null;
    }

    public LevelType? GetNext(LevelType type)
    {
        int index = Array.IndexOf(_levels, type);

        return index >= 0 && index < _levels.Length - 1
            ? _levels[index + 1]
            : null;
    }
}