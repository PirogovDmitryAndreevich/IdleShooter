using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView_6))]
public class LevelView_6Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelView_6 levelView = (LevelView_6)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
        {
            levelView.ZombiesSpawnPoints =
                levelView.GetComponentsInChildren<ZombieSpawnPoint>(false);

            EditorUtility.SetDirty(target);
        }
    }
}
