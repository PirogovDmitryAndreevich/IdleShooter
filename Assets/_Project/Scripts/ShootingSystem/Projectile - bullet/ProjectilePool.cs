using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private float _maxLifetime = 5f;

    private Queue<Projectile> _pool = new Queue<Projectile>();

    public float MaxLifetime => _maxLifetime;

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Create();
        }
    }

    public Projectile Get()
    {
        if(_pool.Count == 0)            
            Create();
        
        Projectile projectile = _pool.Dequeue();
        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void Return(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _pool.Enqueue(projectile);
    }

    private void Create()
    {
        Projectile projectile = Instantiate(_projectilePrefab, transform);
        projectile.gameObject.SetActive(false);
        _pool.Enqueue(projectile);
    }
}