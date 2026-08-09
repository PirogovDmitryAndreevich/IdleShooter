using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Transform _shootPoint;

    public WeaponType Type => _weaponType;
    public Transform ShootPoint => _shootPoint;
}
