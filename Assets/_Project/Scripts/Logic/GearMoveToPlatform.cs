using System;
using System.Collections;
using UnityEngine;

public class GearMoveToPlatform : IDisposable, IResetObject
{
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly ObstacleView _gear;
    private readonly FlyPlatform _platform;
    private readonly Vector3 _gearStartPosition;
    private readonly float _speed;

    private Coroutine _moving;

    public GearMoveToPlatform(ICoroutineRunner coroutineRunner ,ObstacleView gear, FlyPlatform platform, float speed)
    {
        _coroutineRunner = coroutineRunner;
        _gear = gear;
        _platform = platform;
        _speed = speed;

        _gearStartPosition = gear.transform.position;

        _platform.StartMoving += StartMoving;
    }

    public void Dispose() => 
        _platform.StartMoving -= StartMoving;

    public void ResetObject() => 
        _gear.transform.position = _gearStartPosition;

    private void StartMoving(Vector3 endPosition)
    {
        if (!_gear.Mesh.activeInHierarchy)
            return;

        Vector3 targetPosition = new Vector3(endPosition.x, _gear.transform.position.y, endPosition.z);

        StopMoving();
        _moving = _coroutineRunner.StartCoroutine(Moving(_gear.transform.position, targetPosition, _speed));
    }

    private IEnumerator Moving(Vector3 startPosition, Vector3 endPosition, float speed)
    {
        if (!_gear.Mesh.activeInHierarchy)
            yield break;

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / speed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            _gear.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            if (!_gear.Mesh.activeInHierarchy)
                yield break;

            yield return null;
        }

        _gear.transform.position = endPosition;
        _moving = null;
    }

    private void StopMoving()
    {
        if (_moving != null)
        {
            _coroutineRunner.StopCoroutine(_moving);
            _moving = null;
        }
    }
}