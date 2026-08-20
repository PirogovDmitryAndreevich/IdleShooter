using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStaticData))]
public class GameStaticDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameStaticData gameData = (GameStaticData)target;

        if (GUILayout.Button("Build"))
        {
            gameData.LobbySpawnPosition = FindObjectOfType<LobbySpawnPoint>()
                .transform.position;
        }

        EditorUtility.SetDirty(target);
    }
}