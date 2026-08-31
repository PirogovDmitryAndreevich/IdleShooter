using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView_4))]
public class LevelView_4Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelView_4 levelView = (LevelView_4)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
        {
            levelView.DemonSpawnPoint =
                levelView.GetComponentInChildren<DemonSpawnPoint>(false);

            EditorUtility.SetDirty(target);
        }
    }
}
