using Assets._Project.Scripts.SpawnPoints.Obstacles;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetSpawnPoint))]
public class TargetSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(TargetSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, 0.5f);
    }
}
