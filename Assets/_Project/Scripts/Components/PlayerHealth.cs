using System;

public class PlayerHealth : Health, IDisposable
{
    private readonly ILevelManager _levelManager;

    public PlayerHealth(ILevelManager levelManager) : base(levelManager.PlayerHealth)
    {
        _levelManager = levelManager;

        _levelManager.HealthViewUpdated += UpdateHealth;
    }

    public void Dispose() => 
        _levelManager.HealthViewUpdated -= UpdateHealth;

    private void UpdateHealth(int maxHealth) => 
        IncreaseMaxHealth(maxHealth);
}