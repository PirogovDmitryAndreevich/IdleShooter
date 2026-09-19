using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RobotSpawnPoint))]
public class RobotSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(RobotSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, 0.5f);
    }
}
