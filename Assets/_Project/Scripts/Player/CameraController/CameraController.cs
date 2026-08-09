using UnityEngine;
public class CameraController
{
    private readonly IInputService _inputService;
    private readonly IGameSaveDataService _gameSaveData;
    private readonly GameStaticData _gameStaticData;
    private readonly Transform _target;
    private readonly Transform _transformCamera;
    private readonly Transform _parent;

    float _mouseX;
    float _mouseY;

    public Camera Camera { get; private set; }

    public CameraController(PlayerView player, Camera camera, IInputService inputService,
        IGameSaveDataService gameSaveDataService, GameStaticData gameStaticData)
    {
        _target = player.CameraTarget;
        _inputService = inputService;
        _gameSaveData = gameSaveDataService;
        _gameStaticData = gameStaticData;
        Camera = camera;
        _transformCamera = camera.transform;

        GameObject parent = new GameObject("CameraController");
        _parent = parent.transform;

        _transformCamera.SetParent(_parent);
        _transformCamera.localPosition = new Vector3(0, 1.5f, -2f);
        _transformCamera.localRotation = Quaternion.Euler(30f, 0, 0);
    }

    public void Update()
    {
        if (_inputService.ZoomInput != 0)
            ZoomCamera(_inputService.ZoomInput);

        if (_inputService.IsCameraMoving)
            RotateCamera(_inputService.LookInputVector);
    }

    public void LateUpdate()
    {
        MoveCamera();
        UpdateCollision();
    }

    private void UpdateCollision()
    {
        Vector3 origin = _target.position;

        Vector3 desiredPosition = _parent.TransformPoint(0f, 0f, -_gameStaticData.MaxDistance);

        Vector3 direction = desiredPosition - origin;
        float distance = direction.magnitude;

        direction.Normalize();

        Vector3 finalPosition = desiredPosition;

        if (Physics.SphereCast(
            origin,
            _gameStaticData.SphereRadius,
            direction,
            out RaycastHit hit,
            distance,
            _gameStaticData.CollisionMask,
            QueryTriggerInteraction.Ignore))
        {
            finalPosition = hit.point - direction * _gameStaticData.CollisionOffset;
        }

        _transformCamera.position = Vector3.Lerp(
            _transformCamera.position,
            finalPosition,
            Time.deltaTime * 15f);
    }

    private void MoveCamera()
    {
        _parent.position = _target.position + new Vector3(0, _gameStaticData.OffsetDistanceY, 0);
    }

    private void ZoomCamera(float scroll)
    {
        Camera.main.fieldOfView -= scroll * _gameSaveData.Sensitivity * 2;
    }

    private void RotateCamera(Vector2 axis)
    {
        _mouseX += axis.x * _gameSaveData.Sensitivity;
        _mouseY += axis.y * _gameSaveData.Sensitivity;

        _mouseY = Mathf.Clamp(_mouseY, _gameStaticData.CameraLimit.x, _gameStaticData.CameraLimit.y);

        _parent.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);
    }
}