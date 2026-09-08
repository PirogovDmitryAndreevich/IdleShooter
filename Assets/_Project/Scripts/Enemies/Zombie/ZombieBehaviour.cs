using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    private ZombiePresenter _zombie;

    public void SetPresenter(ZombiePresenter zombie) =>
        _zombie = zombie;

    private void Update() =>
        _zombie?.Update();
}