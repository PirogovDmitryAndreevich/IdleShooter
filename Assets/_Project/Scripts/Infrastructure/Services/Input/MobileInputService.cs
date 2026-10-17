using UnityEngine;

public class MobileInputService : InputService
{
    private const string InteractButton = "Interact";
    private const string JumpButton = "Jump";
    private const string ShotButton = "Shot";

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
        GetShotButtonDown();

    public override float ZoomInput =>
        _sliderZoom.GetZoomInput();

    public override bool JumpPressed =>
        SimpleInput.GetButtonDown(JumpButton); 

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

    public override bool GetInteractButtonDown() => 
        SimpleInput.GetButtonDown(InteractButton);

    public bool GetShotButtonDown() =>
        SimpleInput.GetButtonDown(ShotButton);
}
