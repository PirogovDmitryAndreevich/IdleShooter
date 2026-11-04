using UnityEngine;

public class MobileInputService : InputService
{
    private const string JumpButton = "Jump";
    private const string ShootButton = "Shoot";

    private bool _isSwitchAimButtonPressed;

    private readonly ISliderZoomController _sliderZoom;

    public MobileInputService(ISliderZoomController sliderZoom) => 
        _sliderZoom = sliderZoom;

    public override bool IsCameraMoving =>
        LookInputVector != Vector3.zero;

    public override Vector2 Axis =>
        SimpleInputAxis();

    public override Vector3 LookInputVector =>
        SimpleInputLookAxis();

    public override bool IsShoot =>
        SimpleInput.GetButton(ShootButton);

    public override float ZoomInput =>
        _sliderZoom.GetZoomInput();

    public override bool JumpPressed =>
        SimpleInput.GetButtonDown(JumpButton);

    public override bool IsSwitchAimPressed
    {
        get
        {
            bool isPressed = _isSwitchAimButtonPressed;

            _isSwitchAimButtonPressed = false;

            return isPressed;
        }
    }

    public override bool SwitchButtonPressed() =>
        _isSwitchAimButtonPressed = true;

    private Vector2 SimpleInputAxis() =>
        new Vector2
        (
            SimpleInput.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            SimpleInput.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );

    private Vector3 SimpleInputLookAxis() =>
        new Vector2
            (
                SimpleInput.GetAxis(ConstantsData.InputAxis.JoystickX),
                SimpleInput.GetAxis(ConstantsData.InputAxis.JoystickY)
            );
}
