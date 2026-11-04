using UnityEngine;
using UnityEngine.EventSystems;

public class StandaloneInputService : InputService
{
    private readonly ButtonsKeyCodes _buttonsKeyCodes;

    private bool _isSwitchAimButtonPressed;

    public StandaloneInputService(ButtonsKeyCodes buttonsKeyCodes) =>
        _buttonsKeyCodes = buttonsKeyCodes;

    public override bool IsCameraMoving =>
        Input.GetMouseButton(1);

    public override Vector2 Axis =>
        UnityAxis();

    public override Vector3 LookInputVector =>
        MouseInputAxis();

    public override bool IsShoot
    {
        get
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;

            return Input.GetMouseButton(0);
        }
    }

    public override float ZoomInput =>
        Input.GetAxis(ConstantsData.InputAxis.MouseScroll);

    public override bool JumpPressed =>
        Input.GetKeyDown(_buttonsKeyCodes.Jump);

    public override bool IsSwitchAimPressed
    {
        get
        {
            bool isPressed =
                Input.GetKeyDown(_buttonsKeyCodes.SwitchAim) ||
                _isSwitchAimButtonPressed;

            _isSwitchAimButtonPressed = false;

            return isPressed;
        }
    }

    private Vector2 UnityAxis() =>
            new Vector2
            (
                Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
                Input.GetAxis(ConstantsData.InputAxis.VerticalAxis)
            );

    private Vector3 MouseInputAxis() =>
        new Vector2
            (
                Input.GetAxis(ConstantsData.InputAxis.MouseX),
                Input.GetAxis(ConstantsData.InputAxis.MouseY)
            );

    public override bool SwitchButtonPressed() =>
        _isSwitchAimButtonPressed = true;
}