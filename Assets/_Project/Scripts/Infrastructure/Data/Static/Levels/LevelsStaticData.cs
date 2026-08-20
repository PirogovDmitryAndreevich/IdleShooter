using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "StaticData/Level")]
public class LevelsStaticData : ScriptableObject
{
    public LevelType Type;
    public int Recommended;
    public int Wins;
}