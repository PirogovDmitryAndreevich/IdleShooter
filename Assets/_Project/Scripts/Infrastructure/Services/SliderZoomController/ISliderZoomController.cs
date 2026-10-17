public interface ISliderZoomController : IService
{
    void SetPreviousValue(float previousValue);
    float GetZoomInput();
    void OnSliderValueChanged(float value);
}