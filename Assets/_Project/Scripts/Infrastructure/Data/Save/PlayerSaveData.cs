using System;

[Serializable]
public class PlayerSaveData
{
    private const int StartValue = 1;

    public float Damage;
    public int Level;
    public float CurrentScore;
    public int Health;

    public PlayerSaveData()
    {
        Damage = StartValue;
        Level = StartValue;
        CurrentScore = 0f;
        Health = 100;
    }
}