using Assets._Project.Scripts.SpawnPoints.Obstacles;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelView_1))]
public class LevelView_1Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelView_1 levelView = (LevelView_1)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
        {
            FindTargets(levelView);

            EditorUtility.SetDirty(target);
        }
    }

    private void FindTargets(LevelView_1 levelView)
    {
        levelView.TargetsSpawnPoints = levelView.GetComponentsInChildren<TargetSpawnPoint>(false);
    }
}
