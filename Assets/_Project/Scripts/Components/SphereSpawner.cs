using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 1f;

    [Header("Sphere Settings")]
    [SerializeField] private float _knockbackMultiplier = 2f;
    [SerializeField] private float _maxKnockbackForce = 20f;

    private Coroutine _spawnCoroutine;
    private List<SphereObstacle> _spawnedSpheres = new();
    private SphereSpawnPoint[] _spawnPoints;

    private IPoolService _poolService;

    private void OnEnable()
    {
        StopSpawnSphere();
        _spawnCoroutine = StartCoroutine(SpawnSphereCoroutine());
    }

    private void OnDisable()
    {
        StopSpawnSphere();
    }

    public void Construct(IPoolService poolService, SphereSpawnPoint[] spawnPoints)
    {
        _poolService = poolService;
        _spawnPoints = spawnPoints;
    }

    private IEnumerator SpawnSphereCoroutine()
    {
        WaitForSeconds instruction = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            SpawnSpheres();
            yield return instruction;
        }
    }

    private void SpawnSpheres()
    {
        if(_poolService == null)
            return;

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            SphereSpawnPoint spawnPoint = _spawnPoints[i];

            SphereObstacle sphere = _poolService.Get<SphereObstacle>();

            sphere.Construct(_knockbackMultiplier, _maxKnockbackForce);
            sphere.ReachedFinish += OnSphereReachedFinish;

            sphere.transform.SetPositionAndRotation(
            spawnPoint.transform.position,
            spawnPoint.transform.rotation);

            sphere.ResetPhysics();

            _spawnedSpheres.Add(sphere);
        }
    }

    private void StopSpawnSphere()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        for (int i = 0; i < _spawnedSpheres.Count; i++)
        {
            SphereObstacle sphere = _spawnedSpheres[i];

            sphere.ReachedFinish -= OnSphereReachedFinish;

            _poolService.Return(sphere);
        }

        _spawnedSpheres.Clear();
    }

    private void OnSphereReachedFinish(SphereObstacle sphere)
    {
        sphere.ReachedFinish -= OnSphereReachedFinish;

        _spawnedSpheres.Remove(sphere);

        _poolService.Return(sphere);
    }
}
