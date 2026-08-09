using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;

    private PlayerPresenter _presenter;

    public PlayerView View => _playerView;

    void FixedUpdate() => 
        _presenter?.FixedUpdate();

    void Update() =>
        _presenter?.Update();

    private void LateUpdate() => 
        _presenter?.LateUpdate();

    void OnDestroy() => 
        _presenter?.Dispose();

    public void Initialize(PlayerPresenter presenter) =>
       _presenter = presenter;
}