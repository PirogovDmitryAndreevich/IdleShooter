using System;
using UnityEngine;

[Serializable]
public class ShootShakeSettings
{
    [Header("Position Shake")]
    public ShakeSetting PositionShakeSettings;    

    [Header("Rotation Shake")]
    public ShakeSetting RotationShakeSettings;
}
