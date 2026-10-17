using System;

[Serializable]
public class TutorialSaveData
{
    public bool IsTutorialComplete;

    public bool IsWeaponSwitcherStudied;
    public bool IsAimButtonStudied;
    public bool IsPassiveShootingStudied;
    public bool IsGameLevelStudied;
    public bool IsLevelUpStudied;

    public TutorialSaveData()
    {
        IsTutorialComplete = false;
        IsWeaponSwitcherStudied = false;
        IsAimButtonStudied = false;
        IsPassiveShootingStudied = false;
        IsGameLevelStudied = false;
        IsLevelUpStudied = false;
    }
}