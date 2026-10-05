using UnityEngine;

public interface IShooter
{
    void Shoot(float damage);
    void ShootPassive(Vector3 targetPoint);
}