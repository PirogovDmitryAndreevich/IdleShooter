using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    private GhostPresenter _ghost;

    public void SetPresenter(GhostPresenter ghost) =>
        _ghost = ghost;

    private void Update() =>
        _ghost?.Update();
}
