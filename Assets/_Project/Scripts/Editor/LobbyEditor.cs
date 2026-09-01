using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Lobby))]
public class LobbyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Lobby levelView = (Lobby)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
        {
            FindTargets(levelView);

            EditorUtility.SetDirty(target);
        }
    }

    private void FindTargets(Lobby levelView)
    {
        levelView.PlayerSpawnPoint =
            levelView.GetComponentInChildren<PlayerSpawnPoint>(false);

        levelView.WeaponSwitcherSpawnPoint =
            levelView.GetComponentsInChildren<WeaponSwitcherSpawnPoint>(false);

        levelView.WarpSpawnPoint =
            levelView.GetComponentsInChildren<WarpSpawnPoint>(false);
    }
}