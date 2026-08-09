using System;

[Serializable]
public class GameSaveData
{
    public float Sensitivity;

    public GameSaveData()
    {
        Sensitivity = 5f;
    }
}