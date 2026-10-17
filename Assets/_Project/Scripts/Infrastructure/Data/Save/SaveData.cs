using System;

[Serializable]
public class SaveData
{
    public PlayerSaveData PlayerData;
    public WeaponsSaveData Weapons;
    public GameSaveData Game;
    public SettingSaveData Settings;
    public RewardsSaveData Rewards;
    public TutorialSaveData Tutorial;

    public SaveData()
    {
        PlayerData = new PlayerSaveData();
        Weapons = new WeaponsSaveData();
        Game = new GameSaveData();
        Settings = new SettingSaveData();
        Rewards = new RewardsSaveData();
        Tutorial = new TutorialSaveData();
    }
}
