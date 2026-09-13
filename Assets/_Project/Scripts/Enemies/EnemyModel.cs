public class EnemyModel
{
    private readonly EnemyStaticData _data;

    public EnemyModel(EnemyStaticData data)
    {
        _data = data;
    }

    public EnemyType Type => _data.Type;
    public float Damage => _data.Damage;
    public int HealthPoint => _data.HealthPoints;
    public float AttackDistance => _data.AttackDistance;
    public float ShootRange => _data.ShootRange;
    public float RotateSpeed => _data.RotateSpeed;
    public float MoveSpeed => _data.MoveSpeed;
    public float StopMoveDistance => _data.StopMoveDistance;
    public float FireRate => _data.FireRate;
    public ProjectileType ProjectileType =>
        _data.ProjectileType;
}