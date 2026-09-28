using System;

public class DamageManager
{
    private readonly IProgressService _progress;

    public DamageManager(IProgressService progress)
    {
        _progress = progress;
    }

    public event Action<float> DamageChanged;

    public float Damage
    {
        get => _progress.Data.PlayerData.Damage;

        private set 
        {
            if (value == _progress.Data.PlayerData.Damage)
                return;

            _progress.Data.PlayerData.Damage = value;
            DamageChanged?.Invoke(value);
        }
    }

    public void PlayerShot()
    {
        Damage++;
    }
}