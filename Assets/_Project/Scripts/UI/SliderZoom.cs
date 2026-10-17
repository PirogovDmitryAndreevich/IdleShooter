using UnityEngine;
using UnityEngine.UI;

public class SliderZoom : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private ISliderZoomController _sliderZoom;

    public void Construct(ISliderZoomController sliderZoom, IStaticDataService staticDataService)
    {
        _sliderZoom = sliderZoom;
        GameStaticData data = staticDataService.GetGameData();

        _slider.onValueChanged.AddListener(
            _sliderZoom.OnSliderValueChanged);

        _slider.minValue = data.CameraSettingsData.MinFov;
        _slider.maxValue = data.CameraSettingsData.MaxFov;

        _slider.value = Camera.main.fieldOfView;

        _sliderZoom.SetPreviousValue(_slider.value);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(
            _sliderZoom.OnSliderValueChanged);
    }
}