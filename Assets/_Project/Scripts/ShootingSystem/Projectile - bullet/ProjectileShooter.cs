using UnityEngine;

public class ProjectileShooter : IShooter
{
    private readonly WeaponView _weaponView;
    private readonly WeaponModel _model;
    private readonly LayerMask _aimMask;
    private readonly Camera _playerCamera;
    private readonly IProjectilePool _projectilePool;

    public ProjectileShooter(LayerMask aimMask, Camera camera, WeaponModel model,
        WeaponView view, IProjectilePool projectilePool)
    {
        _model = model;
        _weaponView = view;
        _playerCamera = camera; 
        _projectilePool = projectilePool;

        _aimMask = aimMask;

        if (_playerCamera == null)
            _playerCamera = Camera.main;
    }

    public void Shoot(float damage)
    {
        Vector3 aimPoint = GetAimPointFromCamera();
        Vector3 direction = (aimPoint - _weaponView.ShootPoint.position).normalized;

        Projectile projectile = _projectilePool.Get(_model.ProjectileType);

        projectile.transform.position = _weaponView.ShootPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.Shoot(damage, _weaponView.ShootPoint.position, aimPoint);
    }

    public void ShootPassive(Vector3 targetPoint)
    {
        Vector3 hitPoint = GetHitPointFromShootPoint(targetPoint);
        Vector3 direction = (hitPoint - _weaponView.ShootPoint.position).normalized;

        Projectile projectile = _projectilePool.Get(_model.ProjectileType);

        projectile.transform.position = _weaponView.ShootPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.Shoot(0f, _weaponView.ShootPoint.position, hitPoint);
    }

    private Vector3 GetAimPointFromCamera()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _model.MaxAimDistance, _aimMask))        
            return hit.point;       

        return ray.origin + ray.direction * _model.MaxAimDistance;
    }

    private Vector3 GetHitPointFromShootPoint(Vector3 targetPoint)
    {
        Vector3 origin = _weaponView.ShootPoint.position;

        Vector3 direction =
            (targetPoint - origin).normalized;

        float distance =
            Vector3.Distance(origin, targetPoint);

        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            distance,
            _aimMask))
        {
            return hit.point;
        }

        return targetPoint;
    }
}
