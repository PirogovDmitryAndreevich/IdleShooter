using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoldierSpawnPoint))]
public class SoldierSpawnPointEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SoldierSpawnPoint spawnPoint, GizmoType gizmo)
    {
        Gizmos.color = spawnPoint.Color;
        Gizmos.DrawSphere(spawnPoint.transform.position, 0.5f);

        var attackPoint = spawnPoint.AttackPoint;

        if (attackPoint != null)
        {
            Gizmos.color = attackPoint.Color;
            Gizmos.DrawSphere(attackPoint.transform.position, 0.5f);
        }        
    }
}
