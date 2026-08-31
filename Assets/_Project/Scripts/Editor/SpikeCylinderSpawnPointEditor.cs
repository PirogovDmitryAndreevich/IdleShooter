using Assets._Project.Scripts.SpawnPoints.Obstacles;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpikeCylinderSpawnPoint))]
public class SpikeCylinderSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SpikeCylinderSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, 0.5f);
    }
}
