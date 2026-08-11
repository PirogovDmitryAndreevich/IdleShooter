using System;
using UnityEngine;

[Serializable]
public class ShootShakeSettings
{
    [Tooltip("ѕродолжительность тр€ски камеры в секундах.")]
    public float Duration = 0.15f;

    [Tooltip("—ила смещени€ камеры во врем€ тр€ски.")]
    public float PositionStrength = 0.1f;

    [Tooltip("—ила вращени€ камеры во врем€ тр€ски. ќпредел€ет максимальный угол отклонени€.")]
    public float RotationStrength = 2f;

    [Tooltip(" оличество колебаний камеры за врем€ тр€ски. „ем выше значение, тем чаще происходит вибраци€.")]
    public int Vibrato = 20;

    [Tooltip("—тепень случайности направлени€ тр€ски. 0 Ч более равномерное движение, 90 Ч полностью случайное.")]
    [Range(0f, 90f)]
    public float Randomness = 90f;

    [Tooltip("ѕлавно уменьшает силу тр€ски к концу анимации.")]
    public bool FadeOut = true;
}