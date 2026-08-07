
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStaticData", menuName = "StaticData/PlayerStaticData")]
public class PlayerStaticData : ScriptableObject
{
    [Header("Movement Settings")]
    [Tooltip("Скорость передвижения персонажа. Не зависит от гравитации и прыжков.")]
    [Min(0.1f)]
    public float Velocity = 5f;

    [Tooltip("Чем выше значение, тем выше сможет прыгнуть персонаж.")]
    [Min(0.1f)]
    public float JumpForce = 18f;

    [Tooltip("Время нахождения в воздухе. Чем выше значение, тем дольше персонаж будет зависать перед падением.")]
    [Min(0.1f)]
    public float JumpTime = 0.85f;

    [Tooltip("Плавность поворота")]
    public float RotationSmoothness = 0.15f;

    [Space]

    [Header("Physics Settings")]
    [Tooltip("Сила гравитации. Отрицательное значение тянет вниз.")]
    public float Gravity = 9.8f;

    [Tooltip("Сила прижимания к земле. Отрицательное значение.")]
    public float GroundStickForce = -0.5f;

    [Space]

    [Header("Ground Detection")]
    [Tooltip("Дистанция проверки удара головой.")]
    public float HeadHitDistance = 1.1f;

    [Tooltip("Минимальная скорость для определения движения.")]
    public float MovementThreshold = 0.9f;

    private void OnValidate()
    {

        if (GroundStickForce > 0)
        {
            Debug.LogWarning($"GroundStickForce should be negative! Current value: {GroundStickForce}. Auto-correcting...");
            GroundStickForce = -Mathf.Abs(GroundStickForce);
        }

        if (GroundStickForce > -0.01f)
        {
            Debug.LogWarning($"GroundStickForce is too small: {GroundStickForce}. Setting to -0.1f");
            GroundStickForce = -0.1f;
        }

        GroundStickForce = Mathf.Min(-0.01f, GroundStickForce);
    }
}