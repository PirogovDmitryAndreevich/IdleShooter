using System;
using System.Collections.Generic;

[Serializable]
public class WeaponsSaveData
{
    public List<WeaponType> OpenedWeapons;

    public WeaponsSaveData()
    {
        OpenedWeapons = new List<WeaponType>()
        {
            WeaponType.Stones,
            WeaponType.Slingshot,
            WeaponType.Minigun,
            WeaponType.Gun,
            WeaponType.Uzi,
            WeaponType.AK,
            WeaponType.Machine_gun,
            WeaponType.RPG,
        };
    }
}