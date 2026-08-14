using UnityEngine;

public class ProjectileShooter : IShooter
{
    private readonly WeaponView _weaponView;
    private readonly WeaponModel _model;
    private readonly LayerMask _aimMask;
    private readonly Camera _playerCamera;
    private readonly IProjectilePool _projectilePool;
    private readonly IPoolService _poolService;

    public ProjectileShooter(LayerMask aimMask, Camera camera, WeaponModel model,
        WeaponView view, IProjectilePool projectilePool, IPoolService poolService)
    {
        _model = model;
        _weaponView = view;
        _playerCamera = camera; 
        _projectilePool = projectilePool;
        _poolService = poolService;

        _aimMask = aimMask;

        if (_playerCamera == null)
            _playerCamera = Camera.main;
    }

    public void Shoot(float damage)
    {
        Vector3 aimPoint = GetAimPointFromCamera();
        Vector3 direction = (aimPoint - _weaponView.ShootPoint.position).normalized;

        Projectile projectile = _projectilePool.Get(_model.ProjectileType);

        MuzzleEffect muzzleEffect = _poolService.Get<MuzzleEffect>();

        muzzleEffect.Play(_weaponView.ShootPoint, _weaponView.ShootPoint.forward);

        projectile.transform.position = _weaponView.ShootPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.Shoot(damage, _weaponView.ShootPoint.position, aimPoint);
    }    

    private Vector3 GetAimPointFromCamera()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _model.MaxAimDistance, _aimMask))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * _model.MaxAimDistance;
    }
}
