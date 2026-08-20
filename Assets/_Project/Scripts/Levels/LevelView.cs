using UnityEngine;

public abstract class LevelView : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;

    public abstract LevelType LevelType { get; }
    public Transform ExitPoint => _exitPoint;
}
