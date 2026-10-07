using UnityEngine;

public abstract class WeaponView : MonoBehaviour
{
    [Header("WeaponView Settings")]
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private SoundsHandler _soundHandler;

    public WeaponType Type => _weaponType;
    public Transform ShootPoint => _shootPoint;
    public SoundsHandler SoundHandler => _soundHandler;

    public abstract void Shoot();
}
