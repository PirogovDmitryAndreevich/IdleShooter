public class GearModel
{
    private readonly ObstacleStaticData _data;
    private readonly Health _health;

    public GearModel(ObstacleStaticData data)
    {
        _data = data;

        _health = new Health(_data.HealthPoints);
    }

    public Health Health =>
        _health;

    public float Damage =>
        _data.Damage;

    public float MoveSpeed =>
        _data.MoveSpeed;
}
