using System;

public interface IProjectilePool : IService
{
    void Register(
        ProjectileType type,
        Func<Projectile> factory);

    Projectile Get(ProjectileType type);

    public void Return(
        ProjectileType type,
        Projectile projectile);
}