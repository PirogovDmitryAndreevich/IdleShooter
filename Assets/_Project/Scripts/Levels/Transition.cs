using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;

    public Transform ExitPoint => _exitPoint;
}