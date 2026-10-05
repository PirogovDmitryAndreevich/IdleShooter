using UnityEngine;

public class WeaponPresenter : IWeapon
{
    private readonly WeaponModel _model;
    private readonly WeaponView _view;
    private readonly IShooter _shooter;

    private readonly LayerMask _aimMask;
    private readonly Camera _camera;
    private readonly IProjectilePool _projectilePool;
    private readonly IPoolService _pool;

    private float _nextFireTime = 0f;

    public WeaponType Type => _model.Type;
    public WeaponView View => _view;
    public ShootShakeSettings ShakeData => _model.Shake;
    public string AnimationName => _model.AnimationName;
    public float Score => _model.Score;

    public WeaponPresenter(WeaponModel weaponModel, WeaponView view, Camera camera,
        GameStaticData data, IProjectilePool poolService, IPoolService pool)
    {
        _model = weaponModel;
        _view = view;
        _camera = camera;
        _aimMask = data.AimLayerMask;
        _projectilePool = poolService;
        _pool = pool;

        _shooter = CreateShooter(_model.Shooter);
    }

    public bool TryShoot(float damage)
    {
        if (Time.time < _nextFireTime)
            return false;

        _nextFireTime = Time.time + _model.FireRate;
        
        _view.Shoot();
        _shooter.Shoot(damage);

        return true;
    }

    public bool TryShootPassive(Vector3 targetPoint)
    {
        if (Time.time < _nextFireTime)
            return false;

        _nextFireTime = Time.time + _model.FireRate;

        _view.Shoot();
        _shooter.ShootPassive(targetPoint);

        return true;
    }

    public void OnWeaponSelected(IWeapon weapon)
    {
        if (weapon != this)
            return;
    }

    private IShooter CreateShooter(ShooterType type)
    {
        switch (type)
        {
            case ShooterType.Projectile:
                return new ProjectileShooter(_aimMask, _camera, _model, _view, _projectilePool);
            case ShooterType.Raycast:
                return new RaycastShooter(_camera, _aimMask, _model, _view, _pool);
            default:
                throw new System.ArgumentException($"Unknown weapon type: {type}");
        }
    }
}