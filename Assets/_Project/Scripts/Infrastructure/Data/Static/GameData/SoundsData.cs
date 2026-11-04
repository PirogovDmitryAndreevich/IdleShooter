using System;
using UnityEngine;

[Serializable]
public class SoundsData
{
    [Header("UI Sounds")]
    public AudioClip LevelUp;
    public AudioClip OpenWindow;
    public AudioClip CloseWindow;
    public AudioClip Slider;
    public AudioClip GiftButtonCollect;

    [Header("Player Sounds")]
    public AudioClip PlayerStep;
    public AudioClip SelectWeapon;
    public AudioClip Aim;
    public AudioClip Jump;
}
