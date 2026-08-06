using System;

[Serializable]
public class SaveData
{
    public PlayerSaveData PlayerData;
    public SaveData()
    {
        PlayerData = new PlayerSaveData();
    }
}
