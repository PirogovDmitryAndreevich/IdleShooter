using System;

[Serializable]
public class TutorialSaveData
{
    public bool IsTutorialComplete;

    public bool IsAimButtonStudied;
    public bool IsPassiveShootingStudied;
    public bool IsGameLevelStudied;
    public bool IsLevelUpStudied;

    public TutorialSaveData()
    {
        IsTutorialComplete = false;
        IsAimButtonStudied = false;
        IsPassiveShootingStudied = false;
        IsGameLevelStudied = false;
        IsLevelUpStudied = false;
    }
}