using UnityEngine;

public class WeaponPresenter : IWeapon
{
    private readonly WeaponModel _model;
    private readonly WeaponView _view;
    private readonly IShooter _shooter;

    private LayerMask _aimMask;
    private Camera _camera;
    private IPoolService _pool;

    private float _nextFireTime = 0f;

    public WeaponType Type => _model.Type;
    public WeaponView View => _view;

    public ShootShakeSettings Shake => _model.Shake;

    public WeaponPresenter(WeaponModel weaponModel, WeaponView view, Camera camera,
        GameStaticData data, IPoolService poolService)
    {
        _model = weaponModel;
        _view = view;
        _camera = camera;
        _aimMask = data.AimLayerMask;
        _pool = poolService;

        _shooter = CreateShooter(_model.Shooter);
    }

    public bool TryShoot(float damage)
    {
        if (Time.time < _nextFireTime)
            return false;

        _nextFireTime = Time.time + _model.FireRate;
        _shooter.Shoot(damage);

        return true;
    }

    public void OnWeaponSelected(IWeapon weapon)
    {
        if (weapon != this)
            return;
    }

    private IShooter CreateShooter(ShooterType type)
    {
        switch (type
)
        {
            case ShooterType.Projectile:
                return new ProjectileShooter(_aimMask, _camera, _model, _view, _pool);
            case ShooterType.Raycast:
                return new RaycastShooter(_camera, _aimMask, _model);
            default:
                throw new System.ArgumentException($"Unknown weapon type: {type}");
        }
    }
}