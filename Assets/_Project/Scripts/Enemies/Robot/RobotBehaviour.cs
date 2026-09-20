using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    private RobotPresenter _robot;

    public void SetPresenter(RobotPresenter robot) =>
        _robot = robot;

    private void Update() =>
        _robot?.Update();
}