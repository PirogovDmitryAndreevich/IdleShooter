using UnityEngine;

public class DemonBehaviour : MonoBehaviour
{
    private DemonPresenter _demon;

    public void SetPresenter(DemonPresenter demon) => 
        _demon = demon;

    private void Update() => 
        _demon?.Update();
}