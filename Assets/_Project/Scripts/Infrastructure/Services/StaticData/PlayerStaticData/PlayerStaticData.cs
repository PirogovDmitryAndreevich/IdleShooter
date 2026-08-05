
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStaticData", menuName = "StaticData/PlayerStaticData")]
public class PlayerStaticData : ScriptableObject
{
    [Header("Movement Settings")]
    [Tooltip("Скорость передвижения персонажа. Не зависит от гравитации и прыжков.")]
    public float Velocity = 5f;

    [Tooltip("Чем выше значение, тем выше сможет прыгнуть персонаж.")]
    public float JumpForce = 18f;

    [Tooltip("Время нахождения в воздухе. Чем выше значение, тем дольше персонаж будет зависать перед падением.")]
    public float JumpTime = 0.85f;

    [Space]

    [Tooltip("Сила притяжения, тянущая персонажа вниз. Изменение этого значения влияет на движение, прыжки и падение.")]
    public float Gravity = 9.8f;
}