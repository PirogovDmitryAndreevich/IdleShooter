using UnityEngine;

public class CameraZoom
{
    private readonly Camera _camera;
    private readonly IInputService _inputService;
    private readonly IGameSaveDataService _gameSaveData;
    private readonly GameStaticData _gameStaticData;

    private float _targetFov;
    private float _freeFov;

    public CameraZoom(
        Camera camera,
        IInputService inputService,
        IGameSaveDataService gameSaveDataService,
        GameStaticData gameStaticData)
    {
        _camera = camera;
        _inputService = inputService;
        _gameSaveData = gameSaveDataService;
        _gameStaticData = gameStaticData;

        _freeFov = camera.fieldOfView;
        _targetFov = _freeFov;
    }

    public void Update()
    {
        if (_inputService.ZoomInput == 0)
            return;

        _targetFov -=
            _inputService.ZoomInput *
            _gameSaveData.Sensitivity *
            2f;

        _targetFov = Mathf.Clamp(
            _targetFov,
            _gameStaticData.CameraSettingsData.MinFov,
            _gameStaticData.CameraSettingsData.MaxFov);
    }

    public void UpdateFov()
    {
        _camera.fieldOfView = Mathf.Lerp(
            _camera.fieldOfView,
            _targetFov,
            Time.deltaTime *
            _gameStaticData.CameraSettingsData.CameraFovSpeed);
    }

    public void PrepareAim()
    {
        _freeFov = _camera.fieldOfView;

        _targetFov = _gameStaticData.CameraSettingsData.AimFov;
    }

    public void PrepareFree()
    {
        _targetFov = _freeFov;
    }
}