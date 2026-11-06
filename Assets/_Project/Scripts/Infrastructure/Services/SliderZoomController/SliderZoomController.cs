using UnityEngine;

public class SliderZoomController : ISliderZoomController
{
    private const float ZoomMultiplier = 2f;

    private readonly IGameSaveDataService _gameSaveDataService;

    private float _previousValue;
    private float _zoomInput;

    public SliderZoomController(IGameSaveDataService gameSaveDataService) => 
        _gameSaveDataService = gameSaveDataService;

    public void SetPreviousValue(float previousValue)
    {
        _previousValue = previousValue;
        _zoomInput = 0f;
    }

    public float GetZoomInput()
    {
        float input = _zoomInput;
        _zoomInput = 0f;

        return input;
    }

    public void OnSliderValueChanged(float value)
    {
        float delta = value - _previousValue;

        float sensitivity = Mathf.Max(
            _gameSaveDataService.Sensitivity * ZoomMultiplier,
            0.001f);

        _zoomInput -= delta / sensitivity;

        _previousValue = value;
    }
}