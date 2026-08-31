using System;
using System.Collections;
using UnityEngine;

public class FlyPlatform : MonoBehaviour
{
    [SerializeField] private float _flySpeed;
    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    private CharacterController _characterController;
    private bool _isEndPosition;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    public event Action<Vector3> StartMoving;

    private void OnDestroy()
    {
        _triggerObserver.PlayerEntered -= StartMove;
        _triggerObserver.PlayerExited -= RemovePlayer;
    }

    public void Construct(Vector3 endPosition)
    {
        _triggerObserver.PlayerEntered += StartMove;
        _triggerObserver.PlayerExited += RemovePlayer;

        _startPosition = transform.position;
        _endPosition = endPosition;
    }

    private void StartMove(CharacterController characterController)
    {
        _characterController = characterController;

        Vector3 startPosition = _isEndPosition ? _endPosition : _startPosition;
        Vector3 endPosition = _isEndPosition ? _startPosition : _endPosition;

        StartCoroutine(Moving(startPosition, endPosition));

        StartMoving?.Invoke(startPosition);
    }

    private IEnumerator Moving(Vector3 startPosition, Vector3 endPosition)
    {
        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / _flySpeed;

        float elapsedTime = 0f;
        Vector3 previousPosition = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            Vector3 delta = transform.position - previousPosition;

            if (_characterController != null)
                _characterController.Move(delta);

            previousPosition = transform.position;

            yield return null;
        }

        transform.position = endPosition;
        _isEndPosition = !_isEndPosition;
    }

    private void RemovePlayer() => 
        _characterController = null;
}

