using System.Collections.Generic;

public class LevelRegistry
{
    private readonly Dictionary<LevelType, LevelPresenter> _levels = new();

    public bool Contains(LevelType type) =>
        _levels.ContainsKey(type);

    public void Add(LevelPresenter presenter) => 
        _levels[presenter.LevelType] = presenter;

    public LevelPresenter Get(LevelType type) =>
        _levels[type];

    public bool TryGet(LevelType type, out LevelPresenter presenter) =>
        _levels.TryGetValue(type, out presenter);

    public IEnumerable<LevelPresenter> All =>
        _levels.Values;

    public void Clear() => 
        _levels.Clear();
}