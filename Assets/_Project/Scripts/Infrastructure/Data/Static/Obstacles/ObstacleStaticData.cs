using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "StaticData/ObstacleData")]
public class ObstacleStaticData : ScriptableObject
{
    public ObstacleType Type;
    public int HealthPoints = 10;
    public float Damage = 0f;

    [Space]
    public string AssetPath;
}
