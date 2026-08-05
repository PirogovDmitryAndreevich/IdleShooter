using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Tooltip("Чем оно выше, тем быстрее перемещается камера. Рекомендуется увеличить это значение для игр, в которых используется джойстик")]
    [SerializeField] private float _sensitivity = 5f;

    [Tooltip("Пределы поворота камеры по оси Y. Ось X - это максимальное значение, которое она может поднять, а ось Y - максимальное значение, которое она может опустить")]
    [SerializeField] private Vector2 _cameraLimit = new Vector2(-45, 40);

    [SerializeField] private float _offsetDistanceY;

    [Header("Collision")]
    [SerializeField] private float _maxDistance = 5f;
    [SerializeField] private float _sphereRadius = 0.4f;
    [SerializeField] private float _collisionOffset = 0.15f;
    [SerializeField] private LayerMask _collisionMask;

    private IInputService _inputService;
    private Transform _player;
    private Transform _transformCamera;

    float _mouseX;
    float _mouseY;

    void Start()
    {
        _transformCamera = Camera.main.transform;
        _player = GameObject.FindWithTag("Player").transform;
    }

    public void Construct(Transform player, IInputService inputService)
    {
        _player = player;
        _inputService = inputService;
        _transformCamera = Camera.main.transform;

        _transformCamera.SetParent(transform);
        _transformCamera.localPosition = new Vector3(0, 1.5f, -4f);
        _transformCamera.localRotation = Quaternion.Euler(30f, 0, 0);
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetAxisRaw("Fire2") != 0)
            RotateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void LateUpdate()
    {
        MoveCamera();
        UpdateCollision();
    }

    private void UpdateCollision()
    {
        Vector3 origin = _player.position;

        Vector3 desiredPosition = transform.TransformPoint(0f, 0f, -_maxDistance);

        Vector3 direction = desiredPosition - origin;
        float distance = direction.magnitude;

        direction.Normalize();

        Vector3 finalPosition = desiredPosition;

        if (Physics.SphereCast(
            origin,
            _sphereRadius,
            direction,
            out RaycastHit hit,
            distance,
            _collisionMask,
            QueryTriggerInteraction.Ignore))
        {
            finalPosition = hit.point - direction * _collisionOffset;
        }

        _transformCamera.position = Vector3.Lerp(
            _transformCamera.position,
            finalPosition,
            Time.deltaTime * 15f);
    }

    private void MoveCamera()
    {
        transform.position = _player.position + new Vector3(0, _offsetDistanceY, 0);
    }

    private void ZoomCamera(float scroll)
    {
        Camera.main.fieldOfView -= scroll * _sensitivity * 2;
    }

    private void RotateCamera(float mouseX, float mouseY)
    {
        _mouseX += mouseX * _sensitivity;
        _mouseY += mouseY * _sensitivity;

        _mouseY = Mathf.Clamp(_mouseY, _cameraLimit.x, _cameraLimit.y);

        transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || _player == null)
            return;

        Vector3 origin = _player.position;
        Vector3 desiredPosition = transform.TransformPoint(0f, 0f, -_maxDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(origin, 0.08f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(desiredPosition, 0.08f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, desiredPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, _sphereRadius);
        Gizmos.DrawWireSphere(desiredPosition, _sphereRadius);

        if (_transformCamera != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_transformCamera.position, _sphereRadius);
            Gizmos.DrawLine(origin, _transformCamera.position);
        }

        if (Physics.SphereCast(
            origin,
            _sphereRadius,
            (desiredPosition - origin).normalized,
            out RaycastHit hit,
            Vector3.Distance(origin, desiredPosition),
            _collisionMask,
            QueryTriggerInteraction.Ignore))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.12f);
            Gizmos.DrawLine(hit.point, hit.point + hit.normal);
        }
    }
}