using UnityEngine;

[CreateAssetMenu(fileName ="GameStaticData", menuName = "StaticData/GameData")]
public class GameStaticData : ScriptableObject
{
    [Header("CameraSettings")]
    [Tooltip("„ем оно выше, тем быстрее перемещаетс€ камера. –екомендуетс€ увеличить это значение дл€ игр, в которых используетс€ джойстик")]
    public float Sensitivity = 5f;

    [Tooltip("ѕределы поворота камеры по оси Y. ќсь X - это максимальное значение, которое она может подн€ть, а ось Y - максимальное значение, которое она может опустить")]
    public Vector2 CameraLimit = new Vector2(-45, 40);

    [Tooltip("—мещение по оси Y")]
    public float OffsetDistanceY = 2.5f;

    [Header("CollisionCameraSettings")]
    public float MaxDistance = 5f;
    public float SphereRadius = 0.4f;
    public float CollisionOffset = 0.15f;
    public LayerMask CollisionMask;

    [Space]
    [Header("Weapon Settings")]
    [Tooltip("ћаски коллизии дл€ оружи€")]
    public LayerMask AimLayerMask;
}