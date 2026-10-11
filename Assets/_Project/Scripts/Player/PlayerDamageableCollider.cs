using UnityEngine;

public class PlayerDamageableCollider : MonoBehaviour
{

}

public interface IPlayer : IDamageable
{
    void Died();

    void PassiveShooting(IPassiveShooting passiveShooting);

    void OnStopPassiveShooting();
}