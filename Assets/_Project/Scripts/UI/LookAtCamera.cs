using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    private Transform _myTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _myTransform = transform;
    }

    private void Update()
    {
        Quaternion rotation = _cameraTransform.rotation;
        _myTransform.LookAt(_myTransform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}
