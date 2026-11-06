using UnityEngine;
using UnityEngine.UI;

public class SliderZoom : MonoBehaviour
{
    private const float DefaultFov = 45f;

    [SerializeField] private Slider _slider;

    private ISliderZoomController _zoomController;

    public void Construct(ISliderZoomController sliderZoom, IStaticDataService staticDataService)
    {
        _zoomController = sliderZoom;

        GameStaticData data = staticDataService.GetGameData();

        _slider.minValue = data.CameraSettingsData.MinFov;
        _slider.maxValue = data.CameraSettingsData.MaxFov;

        _slider.SetValueWithoutNotify(DefaultFov);

        _zoomController.SetPreviousValue(_slider.value);

        _slider.onValueChanged.AddListener(_zoomController.OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(
            _zoomController.OnSliderValueChanged);
    }
}