using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStaticData))]
public class GameStaticDataEditor : Editor
{
    private const string ObstaclesDataPath = "StaticData/Obstacles";
    private const string WeaponsDataPath = "StaticData/WeaponsData";
    private const string WeaponViewsPath = "StaticData/WeaponView/WeaponViews";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameStaticData gameData = (GameStaticData)target;

        if (GUILayout.Button("Build"))
        {
            gameData.LobbySpawnPosition = FindObjectOfType<LobbySpawnPoint>()
                .transform.position;

            gameData.Obstacles = Resources.LoadAll<ObstacleStaticData>(ObstaclesDataPath);

            gameData.WeaponsStaticData = Resources.LoadAll<WeaponStaticData>(WeaponsDataPath);

            gameData.WeaponsView = Resources.Load<WeaponViewsData>(WeaponViewsPath);

            EditorUtility.SetDirty(target);
        }
    }
}