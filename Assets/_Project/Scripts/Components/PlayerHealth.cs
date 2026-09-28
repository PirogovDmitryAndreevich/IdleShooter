public class PlayerHealth : Health
{
    private readonly PlayerSaveData _saveData;

    public PlayerHealth(IProgressService progress) : base(progress.Data.PlayerData.Health)
    {
        _saveData = progress.Data.PlayerData;
    }


    public void AddHealth(int healthPoints)
    {
        IncreaseMaxHealth(healthPoints);

        _saveData.Health += healthPoints; 
    }
}