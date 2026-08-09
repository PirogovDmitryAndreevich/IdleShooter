using System;

public class PlayerPresenter : IDisposable
{
    private readonly PlayerModel _model;
    private readonly PlayerView _view;
    private readonly PlayerAnimator _animator;
    private readonly IInputService _input;
    private readonly CameraController _camera;
    private readonly WeaponSwitcher _weaponSwitcher;
    private readonly IRotationHandler _rotationHandler;
    private readonly ThirdPersonController _controller;

    private IWeapon _weapon;
    private float _damage = 1f;

    private bool IsWeaponSelected =>
        _weaponSwitcher.CurrentWeapon != null;

    public PlayerPresenter(PlayerModel model, PlayerView view, IInputService inputService,
        CameraController camera, WeaponSwitcher weaponSwitcher)
    {
        _model = model;
        _view = view;
        _animator = new PlayerAnimator(view.Animator);
        _input = inputService;
        _camera = camera;
        _weaponSwitcher = weaponSwitcher;

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

        if (_input.IsShoot && _weapon != null)
            _weapon.TryShoot(_damage);
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

    private void OnWeaponSwitched(IWeapon weapon) => 
        _weapon = weapon;
}