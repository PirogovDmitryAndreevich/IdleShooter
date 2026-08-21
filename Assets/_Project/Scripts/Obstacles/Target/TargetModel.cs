public class TargetModel
{
    private readonly ObstacleStaticData _data;
    private readonly Health _health;

    public TargetModel(ObstacleStaticData data)
    {
        _data = data;

        _health = new Health(_data.HealthPoints);
    }

    public Health Health =>
        _health;
}
