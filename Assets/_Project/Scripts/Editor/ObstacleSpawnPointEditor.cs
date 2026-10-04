using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleSpawnPoint))]
public class ObstacleSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(ObstacleSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, 0.5f);
    }
}
