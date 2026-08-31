using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView_3))]
public class LevelView_3Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelView_3 levelView = (LevelView_3)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
        {
            levelView.SoldierSpawnPoints =
                levelView.GetComponentsInChildren<SoldierSpawnPoint>(false);

            EditorUtility.SetDirty(target);
        }
    }
}
