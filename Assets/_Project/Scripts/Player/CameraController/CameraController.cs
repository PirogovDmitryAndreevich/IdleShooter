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

    private readonly CameraMovement _movement;
    private readonly CameraRotation _rotation;
    private readonly CameraZoom _zoom;
    private readonly CameraCollision _collision;

    private CameraSwitchState _switchState;

    public Camera Camera => _camera;

    public CameraController(
        PlayerView player,
        Camera camera,
        IInputService inputService,
        IGameSaveDataService gameSaveDataService,
        GameStaticData gameStaticData)
    {
        _camera = camera;

        GameObject parent = new GameObject("CameraController");
        camera.transform.SetParent(parent.transform);

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
            camera,
            gameStaticData,
            _movement.Parent);

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
        if (_switchState == CameraSwitchState.ToAim ||
        _switchState == CameraSwitchState.ToFree)
        {
            bool movementFinished = _movement.UpdateTransition();
            bool rotationFinished = _rotation.UpdateTransition(_switchState);

            if (movementFinished && rotationFinished)
            {
                _switchState =
                    _switchState == CameraSwitchState.ToAim
                        ? CameraSwitchState.Aim
                        : CameraSwitchState.Free;
            }
        }
        else
        {
            _movement.Update();
        }

        _collision.Update();
        _zoom.UpdateFov();
    }

    public void SwitchCamera(bool isAim)
    {
        if (isAim)
            SwitchToAim();
        else
            SwitchToFree();
    }

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