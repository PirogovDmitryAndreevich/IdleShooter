using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStaticData))]
public class GameStaticDataEditor : Editor
{
    private const string ObstaclesDataPath = "StaticData/Obstacles";
    private const string WeaponsDataPath = "StaticData/WeaponsData";
    private const string WeaponViewsPath = "StaticData/WeaponView/WeaponViews";
    private const string EnemiesStaticDataPath = "StaticData/Enemies";
    private const string LevelsViewPath = "Levels";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameStaticData gameData = (GameStaticData)target;

        if (GUILayout.Button("Build"))
        {
            gameData.LobbySpawnPosition = FindObjectOfType<LobbySpawnPoint>()
                .transform.position;
            gameData.LevelViews = Resources.LoadAll<LevelView>(LevelsViewPath);

            gameData.Obstacles = Resources.LoadAll<ObstacleStaticData>(ObstaclesDataPath);

            gameData.WeaponsStaticData = Resources.LoadAll<WeaponStaticData>(WeaponsDataPath);

            gameData.WeaponsView = Resources.Load<WeaponViewsData>(WeaponViewsPath);

            gameData.EnemiesStaticData = Resources.LoadAll<EnemyStaticData>(EnemiesStaticDataPath);

            EditorUtility.SetDirty(target);
        }
    }
}