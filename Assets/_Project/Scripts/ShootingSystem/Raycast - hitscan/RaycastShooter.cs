using UnityEngine;

public class RaycastShooter : IShooter
{
    private readonly WeaponModel _model;
    private readonly WeaponView _view;
    private readonly IPoolService _pool;
    private readonly Camera _playerCamera;
    private readonly LayerMask _hitMask;

    public RaycastShooter(Camera playerCamera, LayerMask hitMask, WeaponModel model, WeaponView view, IPoolService pool)
    {
        _playerCamera = playerCamera;
        _hitMask = hitMask;
        _model = model;
        _view = view;
        _pool = pool;

        if (_playerCamera == null)
            _playerCamera = Camera.main;
    }

    public void Shoot(float damage)
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        ShootRealization(damage, ray);
    }

    public void ShootPassive(Vector3 targetPoint)
    {
        Ray ray = new Ray(_view.ShootPoint.position, (targetPoint - _view.ShootPoint.position).normalized);

        ShootRealization(0f, ray);
    }

    private void ShootRealization(float damage, Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _model.MaxAimDistance, _hitMask))
        {
            HotTrail hotTrail = _pool.Get<HotTrail>();
            hotTrail.StartFly(_view.ShootPoint.position, hit);

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable == null)
            {
                damageable = hit.collider.GetComponentInParent<IDamageable>();

                if (damageable == null)
                    damageable = hit.collider.GetComponentInChildren<IDamageable>();
            }

            if (damageable != null)
                damageable.TakeDamage(damage, hit.point, hit.normal);

            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.2f);
        }
    }
}