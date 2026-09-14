using UnityEngine;

public class CannonView : EnemyView
{
    [SerializeField] private WeaponView _weapon;
    [SerializeField] private CannonBehaviour _behaviour;

    public WeaponView Weapon =>
        _weapon;
    public CannonBehaviour Behaviour =>
        _behaviour;
}
