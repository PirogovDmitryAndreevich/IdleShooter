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

        _shake = new CameraShake(
            camera.transform);

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

        _collision.Update();
        _zoom.UpdateFov();
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

    public void Shake(
        float duration,
        float positionStrength,
        float rotationStrength,
        int vibrato = 20,
        float randomness = 90f)
    {
        _shake.Shake(
            duration,
            positionStrength,
            rotationStrength,
            vibrato,
            randomness);
    }

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