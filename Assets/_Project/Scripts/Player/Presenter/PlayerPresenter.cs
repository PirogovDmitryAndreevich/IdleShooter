using System;
using UnityEngine;

public class PlayerPresenter : IDisposable
{
    private readonly PlayerModel _model;
    private readonly PlayerView _view;
    private readonly PlayerAnimator _animator;
    private readonly IInputService _input;
    private readonly IRotationHandler _rotationHandler;
    private readonly ThirdPersonController _controller;

    private bool _isWeaponSelected = true;
    private Camera _playerCamera;

    public PlayerPresenter(PlayerModel model, PlayerView view, IInputService inputService)
    {
        _model = model;
        _view = view;
        _animator = new PlayerAnimator(view.Animator);
        _input = inputService;
        _playerCamera = Camera.main;

        _controller =
            new ThirdPersonController(
                _view.Transform,
                _view.CharacterController,
                _input, _model.StaticData, _playerCamera);

        _rotationHandler = new SmoothRotationHandler();
    }

    public void Update()
    {
        _controller.Update();

        _animator.SetRun(_controller.IsRun);
        _animator.SetJump(_controller.IsJumping);
    }

    public void FixedUpdate()
    {
        _controller.FixedUpdate();

        if (_isWeaponSelected)
        {
            _rotationHandler.RotateTowardsCamera(
                _view.transform,
                _playerCamera,
                _model.StaticData.RotationSmoothness);
        }
        else
        {
            _rotationHandler.RotateTowardsMovement(
                _controller.HorizontalDirection,
                _view.transform,
                _model.StaticData.RotationSmoothness);
        }
        
    }

    public void Dispose()
    {
        _controller.Dispose();
    }
}