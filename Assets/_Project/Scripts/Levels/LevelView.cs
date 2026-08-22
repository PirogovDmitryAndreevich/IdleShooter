using UnityEngine;

public abstract class LevelView : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;

    private LevelPresenter _presenter;

    public abstract LevelType LevelType { get; }
    public Transform ExitPoint => _exitPoint;

    private void OnDestroy() =>
        _presenter?.Dispose();

    public void Construct(LevelPresenter presenter) => 
        _presenter = presenter;
}
