using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "StaticData/Enemy")]
public class EnemyStaticData : ScriptableObject
{
    public EnemyType Type;
    public float Damage;
    public int HealthPoints;
    public float AttackDistance;
    public ProjectileType ProjectileType;
    public float RotateSpeed = 3f;
    public float MoveSpeed = 5f;
    public float StopMoveDistance = 0.3f;
    public float FireRate = 0.5f;
}
