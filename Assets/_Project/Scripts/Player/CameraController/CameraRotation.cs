using UnityEngine;

public class CameraRotation
{
    private readonly IInputService _inputService;
    private readonly IGameSaveDataService _gameSaveData;
    private readonly GameStaticData _gameStaticData;
    private readonly Transform _parent;

    private float _mouseX;
    private float _mouseY;

    private Quaternion _freeRotation;
    private Quaternion _targetRotation;

    public CameraRotation(
        IInputService inputService,
        IGameSaveDataService gameSaveDataService,
        GameStaticData gameStaticData,
        Transform parent)
    {
        _inputService = inputService;
        _gameSaveData = gameSaveDataService;
        _gameStaticData = gameStaticData;
        _parent = parent;

        _freeRotation = _parent.rotation;
        _targetRotation = _freeRotation;
    }

    public void UpdateFree()
    {
        if (!_inputService.IsCameraMoving)
            return;

        Vector2 input = _inputService.LookInputVector;

        _mouseX += input.x * _gameSaveData.Sensitivity;
        _mouseY += input.y * _gameSaveData.Sensitivity;

        _mouseY = Mathf.Clamp(
            _mouseY,
            _gameStaticData.CameraSettingsData.FreeCameraLimit.x,
            _gameStaticData.CameraSettingsData.FreeCameraLimit.y);

        _freeRotation = Quaternion.Euler(
            -_mouseY,
            _mouseX,
            0f);

        _parent.rotation = _freeRotation;
    }

    public void UpdateAim()
    {
        if (!_inputService.IsCameraMoving)
            return;

        Vector2 input = _inputService.LookInputVector;

        _mouseX += input.x * _gameSaveData.Sensitivity;
        _mouseY += input.y * _gameSaveData.Sensitivity;

        _mouseY = Mathf.Clamp(
            _mouseY,
            _gameStaticData.CameraSettingsData.AimCameraLimit.x,
            _gameStaticData.CameraSettingsData.AimCameraLimit.y);

        _parent.rotation = Quaternion.Euler(
            -_mouseY,
            _mouseX,
            0f);
    }

    public void PrepareAim()
    {
        // «апоминаем текущее свободное вращение.
        _freeRotation = _parent.rotation;

        _targetRotation = Quaternion.Euler(
            _gameStaticData.CameraSettingsData.AimRotation);
    }

    public bool UpdateTransition(CameraSwitchState state)
    {
        if (state == CameraSwitchState.ToFree)
        {
            SyncFreeRotation();
            return true;
        }

        _parent.rotation = Quaternion.RotateTowards(
            _parent.rotation,
            _targetRotation,
            _gameStaticData.CameraSettingsData.CameraTransitionRotationSpeed *
            Time.deltaTime);

        if (Quaternion.Angle(
            _parent.rotation,
            _targetRotation) > 0.1f)
        {
            return false;
        }

        _parent.rotation = _targetRotation;

        SyncAimRotation();

        return true;
    }

    private void SyncAimRotation()
    {
        Vector3 euler = _parent.eulerAngles;

        _mouseX = NormalizeAngle(euler.y);
    }

    private void SyncFreeRotation()
    {
        Vector3 euler = _parent.eulerAngles;

        _mouseX = NormalizeAngle(euler.y);
        _mouseY = NormalizeAngle(-euler.x);
    }

    private static float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }
}