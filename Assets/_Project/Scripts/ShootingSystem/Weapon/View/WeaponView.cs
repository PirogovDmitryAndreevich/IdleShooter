using UnityEngine;

public abstract class WeaponView : MonoBehaviour
{
    [Header("WeaponView Settings")]
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private AudioClip _shootSound;

    public WeaponType Type => _weaponType;
    public Transform ShootPoint => _shootPoint;
    public AudioClip ShootSound => _shootSound;

    public abstract void Shoot();
}
