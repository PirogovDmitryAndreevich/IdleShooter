using UnityEngine;

public class HudMobile : Hud
{
    [SerializeField] private SliderZoom _sliderZoom;

    public SliderZoom SliderZoom => _sliderZoom;
}
