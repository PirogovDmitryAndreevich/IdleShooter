using UnityEngine;

public class PlayerDamageableCollider : MonoBehaviour
{

}

public interface IPlayer : IDamageable
{
    void Died();
}

public interface IPassiveShooterInteractor
{
    void PassiveShooting(PassiveShooting passiveShooting);
}