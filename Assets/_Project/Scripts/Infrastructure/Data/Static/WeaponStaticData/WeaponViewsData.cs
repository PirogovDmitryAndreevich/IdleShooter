using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponViews", menuName = "StaticData/WeaponViews")]
public class WeaponViewsData : ScriptableObject
{
    public List<WeaponView> WeaponViews = new List<WeaponView>();
}