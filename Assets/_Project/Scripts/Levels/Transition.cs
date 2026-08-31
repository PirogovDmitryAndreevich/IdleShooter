using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Transform _playerSpawnPoint;

    public LevelType Type { get; private set; }
    public Transform ExitPoint => _exitPoint;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    public void Construct(LevelType type)
    {
        Type = type;
    }
}