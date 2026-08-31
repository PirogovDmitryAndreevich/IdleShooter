using UnityEngine;

public class SpikeCylinderView : Obstacle
{
    [SerializeField] private DamageCollider _damageCollider;

    public DamageCollider DamageCollider =>
        _damageCollider;
}
