using System.Collections;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float _flySpeed;
    [SerializeField] private EndPosition _endPoint;
    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    private Coroutine _moveCoroutine;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float _distance;
    private float _duration;
    private float _inverseDuration;

    private CharacterController _characterController;

    private bool _isEndPosition;

    private void Awake()
    {
        _startPosition = transform.position;
        _endPosition = _endPoint.transform.position;

        _distance = Vector3.Distance(_startPosition, _endPosition);

        _duration = _distance / _flySpeed;
        _inverseDuration = 1f / _duration;
    }

    private void OnEnable()
    {
        _triggerObserver.ControllerEntered += SetPlayer;
        _triggerObserver.PlayerExited += RemovePlayer;

        StartMove();
    }

    private void OnDisable()
    {
        _triggerObserver.ControllerEntered -= SetPlayer;
        _triggerObserver.PlayerExited -= RemovePlayer;

        StopMoveCoroutine();

        _characterController = null;
    }

    public void StartMove()
    {
        StopMoveCoroutine();


        Vector3 startPosition = _isEndPosition
            ? _endPosition
            : _startPosition;

        Vector3 endPosition = _isEndPosition
            ? _startPosition
            : _endPosition;


        _moveCoroutine = StartCoroutine(Moving(startPosition, endPosition));
    }

    private IEnumerator Moving(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0f;
        Vector3 previousPosition = transform.position;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsedTime * _inverseDuration);

            Vector3 currentPosition =
               Vector3.Lerp(startPosition, endPosition, t);

            transform.position = currentPosition;

            if (_characterController != null)
            {
                Vector3 delta = currentPosition - previousPosition;

                _characterController.Move(delta);
            }

            previousPosition = currentPosition;

            yield return null;
        }

        transform.position = endPosition;

        _isEndPosition = !_isEndPosition;

        _moveCoroutine = null;

        StartMove();
    }

    private void SetPlayer(CharacterController characterController) => 
        _characterController = characterController;

    private void StopMoveCoroutine()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }

    private void RemovePlayer() =>
        _characterController = null;
}
