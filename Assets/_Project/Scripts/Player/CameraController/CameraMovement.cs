using UnityEngine;

public class CameraMovement
{
    private readonly PlayerView _playerView;
    private readonly GameStaticData _gameStaticData;

    private readonly Transform _parent;

    private Transform _target;

    public Transform Parent => _parent;

    public CameraMovement(
        PlayerView playerView,
        GameStaticData gameStaticData,
        Transform parent)
    {
        _playerView = playerView;
        _gameStaticData = gameStaticData;                
        _parent = parent;

        _target = _playerView.FreeCameraTarget;
    }

    public void Update()
    {
        _parent.position = _target.position;
    }

    public bool UpdateTransition()
    {
        _parent.position = Vector3.Lerp(
            _parent.position,
            _target.position,
            Time.deltaTime * _gameStaticData.CameraSettingsData.CameraSwitchSpeed);

        return Vector3.Distance(
            _parent.position,
            _target.position) < 0.01f;
    }

    public void SetAim()
    {
        _target = _playerView.AimCameraTarget;
    }

    public void SetFree()
    {
        _target = _playerView.FreeCameraTarget;
    }
}