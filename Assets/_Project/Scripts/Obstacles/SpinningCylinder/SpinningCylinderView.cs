using UnityEngine;

public class SpinningCylinderView : Obstacle
{
    [SerializeField] private DamageCollider _damageCollider;   
    
    public DamageCollider DamageCollider =>
        _damageCollider;
}
