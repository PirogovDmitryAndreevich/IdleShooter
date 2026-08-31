using Assets._Project.Scripts.SpawnPoints.Obstacles;
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
            levelView.TargetsSpawnPoints =
                levelView.GetComponentsInChildren<TargetSpawnPoint>(false);

            levelView.SpinningCylinderSpawnPoint =
                levelView.GetComponentsInChildren<SpinningCylinderSpawnPoint>(false);

            levelView.GearSpawnPoint =
                levelView.GetComponentInChildren<GearSpawnPoint>(false);

            levelView.SpikeCylinderSpawnPoints =
                levelView.GetComponentsInChildren<SpikeCylinderSpawnPoint>(false);

            levelView.GearBaseSpawnPoint =
                levelView.GetComponentInChildren<GearBaseSpawnPoint>(false);

            levelView.FlyPlatformSpawnPoint =
                levelView.GetComponentInChildren<FlyPlatformSpawnPoint>(false);

            EditorUtility.SetDirty(target);
        }
    }
}
