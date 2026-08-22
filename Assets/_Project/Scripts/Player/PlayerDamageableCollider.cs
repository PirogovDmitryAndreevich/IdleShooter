using UnityEngine;

public class PlayerDamageableCollider : MonoBehaviour
{

}

public interface IPlayer
{
    void Died();
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}