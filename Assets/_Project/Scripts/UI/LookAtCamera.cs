using System.Collections;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    private Transform _myTransform;

    private Coroutine _lookCoroutine;

    private void OnEnable()
    {
        _myTransform = transform;

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;

        _lookCoroutine = StartCoroutine(LookAtCameraCoroutine());
    }

    private void OnDisable() => 
        StopLookCoroutine();

    private void OnDestroy() => 
        StopLookCoroutine();

    private IEnumerator LookAtCameraCoroutine()
    {
        while (true)
        {
            if (_cameraTransform != null)
            {
                Quaternion rotation = _cameraTransform.rotation;

                _myTransform.LookAt(
                    _myTransform.position + rotation * Vector3.forward,
                    rotation * Vector3.up);
            }

            yield return null;
        }
    }

    private void StopLookCoroutine()
    {
        if (_lookCoroutine == null)
            return;

        StopCoroutine(_lookCoroutine);
        _lookCoroutine = null;
    }
}
