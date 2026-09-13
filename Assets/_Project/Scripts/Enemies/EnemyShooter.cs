using UnityEngine;

public class EnemyShooter
{
    private const float EyeHeight = 0.7f;

    private readonly EnemyModel _model;
    private readonly WeaponView _view;
    private readonly IProjectilePool _projectilePool;

    private float _nextFireTime = 0f;

    public EnemyShooter(EnemyModel model, WeaponView view, IProjectilePool projectilePool)
    {
        _model = model;
        _view = view;
        _projectilePool = projectilePool;
    }

    public bool TryShoot(Transform target)
    {
        if (Time.time < _nextFireTime)
            return false;

        _nextFireTime = Time.time + _model.FireRate;

        Shoot(target);
        return true;
    }

    private void Shoot(Transform target)
    {
        if (target == null)
            return;

        _view.Shoot();

        Vector3 shootPosition = _view.ShootPoint.position;
        Vector3 endPosition = target.position + Vector3.up * EyeHeight;

        Vector3 direction =
            (endPosition - shootPosition).normalized;

        Projectile projectile =
            _projectilePool.Get(_model.ProjectileType);

        projectile.transform.SetPositionAndRotation(
            shootPosition,
            Quaternion.LookRotation(direction));

        projectile.Shoot(
            _model.Damage,
            shootPosition,
            endPosition);
    }
}