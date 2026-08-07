using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CharacterController _characterController;

    public Animator Animator => _animator;
    public Transform CameraTarget => _cameraTarget;
    public CharacterController CharacterController => _characterController;
    public Transform Transform => transform;
}