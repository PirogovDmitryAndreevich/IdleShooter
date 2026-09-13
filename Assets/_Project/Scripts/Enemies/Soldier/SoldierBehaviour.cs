using UnityEngine;

public class SoldierBehaviour : MonoBehaviour
{
    private SoldierPresenter _soldier;

    public void SetPresenter(SoldierPresenter soldier) =>
        _soldier = soldier;

    private void Update() =>
        _soldier?.Update();
}