using System;
using UnityEngine;

[Serializable]
public class WeaponSwitcherData
{
    public WeaponType WeaponType;
    public Vector3 Position;

    public WeaponSwitcherData(WeaponType weaponType, Vector3 position)
    {
        WeaponType = weaponType;
        Position = position;
    }
}
