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

[CustomEditor(typeof(EndPosition))]
public class EndPositionEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(EndPosition endPoint, GizmoType gizmo)
    {
        Gizmos.color = endPoint.Color;

        Vector3 center = endPoint.transform.position
                 + Vector3.up * endPoint.Size.y / 2f
                 + Vector3.left * endPoint.Size.x / 2f;

        Gizmos.DrawCube(center, endPoint.Size);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(endPoint.transform.position, endPoint.Platform.transform.position);
    }
}
