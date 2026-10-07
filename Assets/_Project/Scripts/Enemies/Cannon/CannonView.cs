using UnityEngine;

public class CannonView : EnemyView
{
    [SerializeField] private WeaponView _weapon;
    [SerializeField] private CannonBehaviour _behaviour;
    [SerializeField] private CannonSounds _sounds;

    public WeaponView Weapon =>
        _weapon;
    public CannonBehaviour Behaviour =>
        _behaviour;
    public CannonSounds Sounds => _sounds;
}
