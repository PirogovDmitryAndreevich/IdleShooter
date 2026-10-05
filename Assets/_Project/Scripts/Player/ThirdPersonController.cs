using System;
using UnityEngine;

public class ThirdPersonController : IDisposable
{
    private const float TeleportYOffset = 0.5f;

    // Компоненты
    private readonly IMovementCalculator _movementCalculator;
    private readonly IJumpHandler _jumpHandler;
    private readonly IHeadHitDetector _headHitDetector;

    // Зависимости
    private readonly Transform _viewTransform;
    private readonly CharacterController _characterController;
    private readonly PlayerStaticData _playerData;
    private readonly IInputService _input;

    private bool _skipNextMovement;

    public ThirdPersonController(
        Transform viewTransform,
        CharacterController characterController,
        IInputService inputService,
        PlayerStaticData data,
        Camera playerCamera)
    {
        _input = inputService;
        _playerData = data;
        _viewTransform = viewTransform;
        _characterController = characterController;

        _movementCalculator = new CameraRelativeMovementCalculator(playerCamera, data);
        _jumpHandler = new DefaultJumpHandler(data);
        _headHitDetector = new HeadHitDetector(data);

        // Подписка на события
        _jumpHandler.OnJumpStarted += OnJumpStartedInternal;
        _jumpHandler.OnJumpEnded += OnJumpEndedInternal;
    }

    // События
    public event Action OnJumpStarted;
    public event Action OnJumpEnded;

    public bool IsRun =>
        _characterController.isGrounded &&
        _characterController.velocity.magnitude > _playerData.MovementThreshold;
    public bool HasMovementInput =>
    _input.Axis.sqrMagnitude > _playerData.MovementThreshold;
    public bool IsJumping => !_characterController.isGrounded;
    public Vector3 HorizontalDirection => _movementCalculator.HorizontalDirection;

    public void Update()
    {
        if (_input.JumpPressed && _characterController.isGrounded && !_jumpHandler.IsJumping)
            _jumpHandler.StartJump();

        if (_headHitDetector.CheckHeadHit(_viewTransform, _characterController))
            _jumpHandler.ForceEndJump();
    }
    public void FixedUpdate()
    {
        if (_skipNextMovement)
        {
            _skipNextMovement = false;
            return;
        }

        _jumpHandler.UpdateJumpDirection();

        _movementCalculator.UpdateMoveDirection(_input.Axis);

        Vector3 movement = _jumpHandler.VerticalDirection + _movementCalculator.HorizontalDirection;
        _characterController.Move(movement);
    }

    public void Teleport(Transform target)
    {
        if (target == null)
            return;

        _jumpHandler.ForceEndJump();

        _characterController.enabled = false;

        _viewTransform.SetPositionAndRotation(
    target.position + Vector3.up * TeleportYOffset,
    target.rotation);

        _characterController.enabled = true;

        Physics.SyncTransforms();

        _skipNextMovement = true;
    }

    public void SetToPoint(Transform target)
    {
        if (target == null)
            return;

        _jumpHandler.ForceEndJump();

        _characterController.enabled = false;

        _viewTransform.SetPositionAndRotation(
                    target.position,
                    target.rotation);

        _characterController.enabled = true;

        Physics.SyncTransforms();

        _skipNextMovement = true;
    }

    public void Dispose()
    {
        _jumpHandler.OnJumpStarted -= OnJumpStartedInternal;
        _jumpHandler.OnJumpEnded -= OnJumpEndedInternal;
    }

    private void OnJumpStartedInternal() =>
        OnJumpStarted?.Invoke();

    private void OnJumpEndedInternal() =>
        OnJumpEnded?.Invoke();
}