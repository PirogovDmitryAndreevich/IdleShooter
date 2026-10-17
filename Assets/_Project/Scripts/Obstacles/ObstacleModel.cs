using UnityEngine;

public class ObstacleModel
{
    private readonly ObstacleStaticData _data;
    private readonly Health _health;

    public ObstacleModel(ObstacleStaticData data)
    {
        _data = data;

        _health = new Health(_data.HealthPoints);
    }

    public Health Health =>
        _health;

    public float Damage =>
        _data.Damage;

    public int Crowns => 
        _data.Crowns;

    public AudioClip TakeDamageSound =>
        _data.TakeDamageSound;
}