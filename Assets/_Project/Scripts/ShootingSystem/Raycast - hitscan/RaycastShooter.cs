using UnityEngine;

public class RaycastShooter : IShooter
{
    private WeaponModel _model;
    private Camera _playerCamera;
    private LayerMask _hitMask;

    public RaycastShooter(Camera playerCamera, LayerMask hitMask, WeaponModel model)
    {
        _playerCamera = playerCamera;
        _hitMask = hitMask;
        _model = model;

        if (_playerCamera == null)
            _playerCamera = Camera.main;
    }

    public void Shoot(float damage)
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _model.MaxAimDistance, _hitMask))
        {
            BaseDamageable damageable = hit.collider.GetComponent<BaseDamageable>();

            if (damageable == null)
            {
                damageable = hit.collider.GetComponentInParent<BaseDamageable>();

                if (damageable == null)
                    damageable = hit.collider.GetComponentInChildren<BaseDamageable>();
            }

            if (damageable != null)
                damageable.TakeDamage(damage, hit.point, hit.normal);

            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.2f);
        }
    }
}