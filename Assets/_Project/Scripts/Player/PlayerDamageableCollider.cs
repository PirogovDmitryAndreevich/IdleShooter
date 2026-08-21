using UnityEngine;

public class PlayerDamageableCollider : MonoBehaviour
{

}

public interface IPlayerDamageable
{
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}