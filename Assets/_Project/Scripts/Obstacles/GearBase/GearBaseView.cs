using UnityEngine;

public class GearBaseView : Obstacle
{
    [SerializeField] private DamageCollider _damageCollider;

    public DamageCollider DamageCollider =>
        _damageCollider;
}
