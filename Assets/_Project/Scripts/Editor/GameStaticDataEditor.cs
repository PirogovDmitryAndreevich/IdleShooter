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
            gameData.WeaponSwitcherData =
                 FindObjectsOfType<WeaponSwitcherSpawnPoint>()
                 .Select(x => new WeaponSwitcherData(x.WeaponType, x.transform.position))
                 .ToArray();
        }

        EditorUtility.SetDirty(target);
    }
}