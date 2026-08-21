using UnityEngine;

public class PlayerDamageableCollider : MonoBehaviour
{

}

public interface IPlayerDamageable
{
    void Died();
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}