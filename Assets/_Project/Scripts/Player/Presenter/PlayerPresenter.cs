using System;
using UnityEngine;

public class PlayerPresenter : IDisposable
{
    private readonly PlayerModel _model;
    private readonly PlayerView _view;
    private readonly PlayerAnimator _animator;
    private readonly IInputService _input;
    private readonly CameraController _camera;
    private readonly WeaponSwitcher _weaponSwitcher;
    private readonly Hud _hud;
    private readonly IRotationHandler _rotationHandler;
    private readonly ThirdPersonController _controller;

    private IWeapon _weapon;
    private float _damage = 1f;
    private bool _isAim;

    private bool IsWeaponSelected =>
        _weaponSwitcher.CurrentWeapon != null;

    public PlayerPresenter(PlayerModel model, PlayerView view, IInputService inputService,
        CameraController camera, WeaponSwitcher weaponSwitcher, Hud hud)
    {
        _model = model;
        _view = view;
        _animator = new PlayerAnimator(view.Animator);
        _input = inputService;
        _camera = camera;
        _weaponSwitcher = weaponSwitcher;
        _hud = hud;

        _controller =
            new ThirdPersonController(
                _view.Transform,
                _view.CharacterController,
                _input, _model.StaticData, _camera.Camera);

        _rotationHandler = new SmoothRotationHandler();

        _weaponSwitcher.WeaponSwitched += OnWeaponSwitched;
    }

    public void FixedUpdate()
    {
        _controller.FixedUpdate();

        if (IsWeaponSelected)
        {
            _rotationHandler.RotateTowardsCamera(
                _view.transform,
                _camera.Camera,
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

    public void Update()
    {
        _controller.Update();
        _camera.Update();       

        _animator.SetRun(_controller.IsRun);
        _animator.SetJump(_controller.IsJumping);

        if (_input.IsShoot && _isAim)        
            Shoot();

        if (Input.GetKeyDown(KeyCode.P))
            _camera.Shake(0.1f, 0.01f, 1.5f);

        if (Input.GetKeyDown(KeyCode.O))
            _camera.Shake(0.25f, 0.08f, 4f);

        if (Input.GetKeyDown(KeyCode.I))
            _camera.Shake(0.4f, 0.15f, 6f, 25, 90f);
    }

    public void LateUpdate()
    {
        _camera.LateUpdate();
    }

    public void Dispose()
    {
        _controller.Dispose();
        _weaponSwitcher.Dispose();
        _weaponSwitcher.WeaponSwitched -= OnWeaponSwitched;
    }

    private void OnWeaponSwitched(IWeapon weapon)
    {
        _weapon = weapon;

        _isAim = _weapon != null;
        _camera?.SwitchCamera(_isAim);
        _hud.OnAimState(_isAim);
    }

    private void Shoot()
    {
        _weapon.TryShoot(_damage);
        _camera.Shake(0.1f, 0.06f, 1.5f);
    }
}