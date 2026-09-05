using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SphereSpawnPoint))]
public class SphereSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SphereSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, spawnPoint.Radius);
    }
}
