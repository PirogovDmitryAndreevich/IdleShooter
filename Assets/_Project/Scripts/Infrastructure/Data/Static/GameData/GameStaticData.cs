using UnityEngine;

[CreateAssetMenu(fileName ="GameStaticData", menuName = "StaticData/GameData")]
public class GameStaticData : ScriptableObject
{
    [Header("Camera Settings")]
    [Tooltip("Чем оно выше, тем быстрее перемещается камера. Рекомендуется увеличить это значение для игр, в которых используется джойстик")]
    public float Sensitivity = 5f;
    [Tooltip("Пределы поворота камеры по оси Y. Ось X - это максимальное значение, которое она может поднять, а ось Y - максимальное значение, которое она может опустить")]
    public Vector2 CameraLimit = new Vector2(-45, 40);
    [Tooltip("Дистанция по оси Z, вычитается от Target")]
    public float MaxDistance = 5f;
    [Tooltip("Смещение по оси Y")]
    public float OffsetDistanceY = 2.5f;
    [Tooltip("Скорость переключения Target камеры")]
    public float CameraSwitchSpeed;

    [Header("Collision Camera Settings")]
    [Tooltip("Радиус сферы камеры")]
    public float SphereRadius = 0.4f;
    [Tooltip("Смещение камеры от точки столкновения с препятствием, нужно чтобы камера не оказывалась в стене")]
    public float CollisionOffset = 0.15f;
    [Tooltip("Скорость смещения камеры")]
    public float CameraCollisionLerpSpeed = 15f;

    [Header("Camera Zoom Settings")]
    [Tooltip("Зуум при прицеливании")]
    public float AimFov;
    [Tooltip("Скорость переключения зума при прицеливании")]
    public float CameraFovSpeed;
    [Tooltip("Минимальное Зум значение")]
    public float MinFov;
    [Tooltip("Максимальное Зум значение")]
    public float MaxFov;

    [Header("Camera Rotation Settings")]
    [Tooltip("Поворот при прицеливании")]
    public Vector3 AimRotation = new Vector3(-17f,0,0);
    [Tooltip("Скорость поворота при переключении")]
    public float CameraTransitionRotationSpeed;
    public LayerMask CollisionMask;

    [Space]
    [Header("Weapon Settings")]
    [Tooltip("Маски коллизии для оружия")]
    public LayerMask AimLayerMask;
}