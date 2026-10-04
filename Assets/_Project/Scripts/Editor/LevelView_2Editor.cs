using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView_2))]
public class LevelView_2Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelView_2 levelView = (LevelView_2)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
        {
            levelView.ObstacleSpawnPoints =
                levelView.GetComponentsInChildren<ObstacleSpawnPoint>(false);

            levelView.FlyPlatformSpawnPoint =
                levelView.GetComponentInChildren<FlyPlatformSpawnPoint>(false);

            EditorUtility.SetDirty(target);
        }
    }
}
