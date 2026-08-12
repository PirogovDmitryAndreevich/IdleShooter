using UnityEngine;

public enum CameraSwitchState
{
    Free,
    ToAim,
    Aim,
    ToFree
}

public class CameraController
{
    private readonly Camera _camera;
    private readonly GameStaticData _gameStaticData;
    private readonly CameraMovement _movement;
    private readonly CameraRotation _rotation;
    private readonly CameraZoom _zoom;
    private readonly CameraCollision _collision;
    private readonly CameraShake _shake;

    private CameraSwitchState _switchState;

    public Camera Camera => _camera;

    public CameraController(
        PlayerView player,
        Camera camera,
        IInputService inputService,
        IGameSaveDataService gameSaveDataService,
        GameStaticData gameStaticData)
    {
        _gameStaticData = gameStaticData;
        _camera = camera;

        GameObject parent = new GameObject("CameraController");
        GameObject shake = new GameObject("CameraShake");

        shake.transform.SetParent(parent.transform);
        shake.transform.localPosition = Vector3.zero;
        shake.transform.localRotation = Quaternion.identity;

        camera.transform.SetParent(shake.transform);

        _movement = new CameraMovement(
            player,
            gameStaticData,
            parent.transform);

        _rotation = new CameraRotation(
            inputService,
            gameSaveDataService,
            gameStaticData,
            _movement.Parent);

        _zoom = new CameraZoom(
            camera,
            inputService,
            gameSaveDataService,
            gameStaticData);

        _collision = new CameraCollision(
            gameStaticData,
            _movement.Parent);

        _shake = new CameraShake(
             shake.transform);

        _switchState = CameraSwitchState.Free;
    }

    public void Update()
    {
        switch (_switchState)
        {
            case CameraSwitchState.Free:
                _rotation.UpdateFree();
                _zoom.Update();
                break;

            case CameraSwitchState.Aim:
                _rotation.UpdateAim();
                break;
        }
    }

    public void LateUpdate()
    {
        UpdateSwitch();

        UpdateCameraPosition();
        _zoom.UpdateFov();
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition =
            _collision.CalculatePosition();

        _camera.transform.position = Vector3.Lerp(
            _camera.transform.position,
            targetPosition,
            Time.deltaTime *
            _gameStaticData.CameraSettingsData.CameraCollisionLerpSpeed);
    }

    private void UpdateSwitch()
    {
        if (_switchState != CameraSwitchState.ToAim &&
            _switchState != CameraSwitchState.ToFree)
        {
            _movement.Update();
            return;
        }

        bool movementFinished =
            _movement.UpdateTransition();

        bool rotationFinished =
            _rotation.UpdateTransition(_switchState);

        if (!movementFinished ||
            !rotationFinished)
        {
            return;
        }

        if (_switchState == CameraSwitchState.ToAim)        
            _switchState = CameraSwitchState.Aim;        
        else        
            _switchState = CameraSwitchState.Free;        
    }

    public void SwitchCamera(bool isAim)
    {
        if (isAim)
            SwitchToAim();
        else
            SwitchToFree();
    }

    public void Shake(ShootShakeSettings settings) => 
        _shake.Shake(settings);

    public void StopShake() => 
        _shake.Stop();

    private void SwitchToAim()
    {
        if (_switchState == CameraSwitchState.Aim ||
            _switchState == CameraSwitchState.ToAim)
            return;

        _rotation.PrepareAim();
        _zoom.PrepareAim();

        _movement.SetAim();

        _switchState = CameraSwitchState.ToAim;
    }

    private void SwitchToFree()
    {
        if (_switchState == CameraSwitchState.Free ||
            _switchState == CameraSwitchState.ToFree)
            return;

        _rotation.PrepareFree();
        _zoom.PrepareFree();

        _movement.SetFree();

        _switchState = CameraSwitchState.ToFree;
    }
}