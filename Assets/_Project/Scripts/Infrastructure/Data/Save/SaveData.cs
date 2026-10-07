using System;

[Serializable]
public class SaveData
{
    public PlayerSaveData PlayerData;
    public WeaponsSaveData Weapons;
    public GameSaveData Game;
    public AudioSettingSaveData Audio;

    public SaveData()
    {
        PlayerData = new PlayerSaveData();
        Weapons = new WeaponsSaveData();
        Game = new GameSaveData();
        Audio = new AudioSettingSaveData();
    }
}
