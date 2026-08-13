using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : IProjectilePool
{
    private readonly Dictionary<ProjectileType, Queue<Projectile>> _pools = new();
    private readonly Dictionary<ProjectileType, Func<Projectile>> _factories = new();
    private readonly Dictionary<Projectile, ProjectileType> _types = new();
    private readonly Dictionary<ProjectileType, Transform> _parents = new();

    public void Register(
        ProjectileType type,
        Func<Projectile> factory)
    {
        _factories[type] = factory;
    }

    public Projectile Get(ProjectileType type)
    {
        if (_pools.TryGetValue(type, out var pool) && pool.Count > 0)
        {
            Projectile projectile = pool.Dequeue();
            projectile.gameObject.SetActive(true);

            return projectile;
        }

        return CreateNew(type);
    }

    public void Return(
        ProjectileType type,
        Projectile projectile)
    {
        if (!_pools.TryGetValue(type, out var pool))
        {
            pool = new Queue<Projectile>();
            _pools[type] = pool;
        }

        projectile.gameObject.SetActive(false);

        pool.Enqueue(projectile);
    }

    private Projectile CreateNew(ProjectileType type)
    {
        if (!_factories.TryGetValue(type, out var factory))
            throw new Exception($"Factory for {type} is not registered.");

        if (!_parents.TryGetValue(type, out var parent))
        {
            parent = CreateParent(type);
            _parents[type] = parent;
        }

        Projectile projectile = factory();

        projectile.transform.SetParent(parent);

        _types[projectile] = type;

        return projectile;
    }

    private Transform CreateParent(ProjectileType type)
    {
        GameObject parent =
            new GameObject($"{type}ProjectilePool");

        return parent.transform;
    }
}