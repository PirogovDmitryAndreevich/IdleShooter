public class SliderZoomController : ISliderZoomController
{
    private float _previousValue;
    private float _zoomInput;

    public void SetPreviousValue(float previousValue) =>
        _previousValue = previousValue;

    public float GetZoomInput()
    {
        float input = _zoomInput;
        _zoomInput = 0f;

        return input;
    }

    public void OnSliderValueChanged(float value)
    {
        float delta = value - _previousValue;

        _zoomInput += delta;

        _previousValue = value;
    }
}