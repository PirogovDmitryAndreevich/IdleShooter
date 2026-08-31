public class SoldierModel
{
    private readonly EnemyStaticData _data;

    public EnemyType Type => _data.Type;
    public float Damage => _data.Damage;
    public float AttackDistance => _data.AttackDistance;
    public float RotateSpeed => _data.RotateSpeed;
    public float MoveSpeed => _data.MoveSpeed;
    public float FireRate => _data.FireRate;
    public ProjectileType ProjectileType =>
        _data.ProjectileType;

    public SoldierModel(EnemyStaticData data)
    {
        _data = data;
    }
}
