using UnityEngine;

public class CannonBehaviour : MonoBehaviour
{
    private CannonPresenter _cannon;

    public void SetPresenter(CannonPresenter cannon) =>
        _cannon = cannon;

    private void Update() =>
        _cannon?.Update();
}