using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class RaycastShooter : MonoBehaviour, IShooter
{
    [Header("RaycastShooter Settings")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _range = 1000f;
    [SerializeField] private LayerMask _hitMask;

    private Weapon _weapon;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();

        if(_playerCamera == null)
            _playerCamera = Camera.main;
    }

    public void Shoot()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _range, _hitMask))
        {
            BaseDamageable damageable = hit.collider.GetComponent<BaseDamageable>();

            if (damageable == null)
            {
                damageable = hit.collider.GetComponentInParent<BaseDamageable>();

                if(damageable == null)                
                    damageable = hit.collider.GetComponentInChildren<BaseDamageable>();                
            }

            if (damageable != null)            
                damageable.TakeDamage(_weapon.Damage, hit.point, hit.normal);         
            
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.2f);
        }
    }
}