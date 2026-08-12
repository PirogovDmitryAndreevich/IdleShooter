using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSwitcherSpawnPoint))]
public class WeaponSwitcherSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(WeaponSwitcherSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnPoint.transform.position, 0.5f);
    }
}
