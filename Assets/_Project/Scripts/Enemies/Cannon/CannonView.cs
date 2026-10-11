using UnityEngine;

public class CannonView : EnemyView
{
    [SerializeField] private WeaponView _weapon;
    [SerializeField] private CannonBehaviour _behaviour;
    [SerializeField] private EnemySounds _sounds;

    public WeaponView Weapon =>
        _weapon;
    public CannonBehaviour Behaviour =>
        _behaviour;
    public EnemySounds Sounds => _sounds;
}
