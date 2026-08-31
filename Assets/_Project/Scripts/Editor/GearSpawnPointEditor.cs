using Assets._Project.Scripts.SpawnPoints.Obstacles;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GearSpawnPoint))]
public class GearSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(GearSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, 1f);
    }
}
