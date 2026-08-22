using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlyPlatformSpawnPoint))]
public class FlyPlatformSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(FlyPlatformSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.SpawnColor;

        Vector3 center = spawnPoint.transform.position
                 + Vector3.up * spawnPoint.Size.y / 2f
                 + Vector3.left * spawnPoint.Size.x / 2f;

        Gizmos.DrawCube(center, spawnPoint.Size);

        Gizmos.color = spawnPoint.EndPointColor;
        
        Vector3 endCenter = spawnPoint.EndPoint.transform.position
                    + Vector3.up * spawnPoint.Size.y / 2f
                    + Vector3.left * spawnPoint.Size.x / 2f;

        Gizmos.DrawCube(endCenter, spawnPoint.Size);
    }
}
