public class WeaponModel
{
    private readonly WeaponStaticData _staticData;

    public WeaponType Type => _staticData.Type;
    public ShooterType Shooter => _staticData.ShooterType;
    public float FireRate => _staticData.FireRate;
    public float MaxAimDistance => _staticData.MaxAimDistance;
    public ShootShakeSettings Shake => _staticData.Shake;

    public WeaponModel(WeaponStaticData staticData)
    {
        _staticData = staticData;
    }
}
