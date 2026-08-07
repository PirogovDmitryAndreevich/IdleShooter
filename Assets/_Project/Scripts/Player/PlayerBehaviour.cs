using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;

    private PlayerPresenter _presenter;

    public PlayerView View => _playerView;

    public void Initialize(PlayerPresenter presenter)
    {
        _presenter = presenter;
    }

    void Update()
    {
        _presenter?.Update();
    }

    void FixedUpdate()
    {
        _presenter?.FixedUpdate();
    }

    void OnDestroy()
    {
        _presenter?.Dispose();
    }
}