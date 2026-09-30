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
    private readonly DamageManager _damageManager;
    private readonly PlayerHealth _health;
    private readonly ILevelManager _level;
    private readonly IRotationHandler _rotationHandler;
    private readonly ThirdPersonController _controller;

    private IWeapon _weapon;
    private bool _isAim;

    private bool HasWeapon => _weapon != null;
    private bool CanShoot => HasWeapon && _isAim;
    public bool IsAiming => _isAim;
    public DamageManager DamageManager => _damageManager;
    public PlayerHealth Health => _health;

    public event Action<bool> AimChanged;
    public event Action<bool> WeaponAvailabilityChanged;
    public event Action PlayerDied;
    public event Action<float> PlayerShot;

    public PlayerPresenter(
        PlayerModel model,
        PlayerView view,
        IInputService inputService,
        CameraController camera,
        WeaponSwitcher weaponSwitcher,
        DamageManager damageManager,
        PlayerHealth health,
        ILevelManager level)
    {
        _model = model;
        _view = view;
        _animator = new PlayerAnimator(view.Animator);
        _input = inputService;
        _camera = camera;
        _weaponSwitcher = weaponSwitcher;
        _damageManager = damageManager;
        _health = health;
        _level = level;

        _controller = new ThirdPersonController(
            _view.Transform,
            _view.CharacterController,
            _input,
            _model.StaticData,
            _camera.Camera);

        _rotationHandler = new SmoothRotationHandler();

        _weaponSwitcher.WeaponSwitched += OnWeaponSwitched;
        _weaponSwitcher.WeaponOpened += OnWeaponOpened;

        _view.Damage += TakeDamage;
        _view.PlayerDied += OnPlayerDied;

        _health.Died += OnPlayerDied;
    }

    public void Dispose()
    {
        _weaponSwitcher.WeaponSwitched -= OnWeaponSwitched;
        _weaponSwitcher.WeaponOpened -= OnWeaponOpened;

        _view.Damage -= TakeDamage;
        _view.PlayerDied -= OnPlayerDied;

        _health.Died -= OnPlayerDied;

        _controller.Dispose();
        _weaponSwitcher.Dispose();
    }

    public void FixedUpdate()
    {
        _controller.FixedUpdate();

        if (_isAim)
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

        if (CanShoot && _input.IsShoot)
            Shoot();
    }

    public void LateUpdate()
    {
        _camera.LateUpdate();
    }

    public void SwitchAim(bool isAim)
    {
        if (isAim && !HasWeapon)
            return;

        if (_isAim == isAim)
            return;

        _isAim = isAim;

        if (HasWeapon)
        {
            _animator.SetWeaponAnimation(
                _weapon.AnimationName,
                _isAim);
        }

        _camera.SwitchCamera(_isAim);

        AimChanged?.Invoke(_isAim);
    }

    public void Teleport(Transform target)
    {
        if (target == null)
            return;

        SwitchAim(false);

        _controller.Teleport(target);

        _camera.SyncWithPlayer(target.rotation);
    }

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _view.HitEffect.Flash();
        _health.TakeDamage(damage);
    }

    private void OnWeaponSwitched(IWeapon weapon)
    {
        if (_weapon != null)
        {
            _animator.SetWeaponAnimation(
                _weapon.AnimationName,
                false);
        }

        _weapon = weapon;
        _view.SwitchWeapon(_weapon);

        if (_weapon == null)
        {
            SwitchAim(false);

            WeaponAvailabilityChanged?.Invoke(false);
            return;
        }

        WeaponAvailabilityChanged?.Invoke(true);

        if (_isAim)
        {
            _animator.SetWeaponAnimation(
                _weapon.AnimationName,
                true);
        }
    }

    private void OnWeaponOpened(IWeapon weapon)
    {
        _view.OpenWeapon(weapon);
    }

    private void Shoot()
    {
        if (!CanShoot)
            return;

        if (_weapon.TryShoot(_damageManager.Damage))
        {
            _damageManager.PlayerShot();
            _level.AddScore(_weapon.Score);
            _camera.Shake(_weapon.ShakeData);
            PlayerShot?.Invoke(_weapon.Score);
        }
    }

    private void OnPlayerDied() =>
        PlayerDied?.Invoke();
}