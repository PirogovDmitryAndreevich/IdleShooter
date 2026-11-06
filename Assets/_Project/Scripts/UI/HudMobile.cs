using UnityEngine;

public class HudMobile : Hud
{
    [SerializeField] private SliderZoom _sliderZoom;
    [SerializeField] private MoveRunSwitcher _moveRunSwitcher;

    public SliderZoom SliderZoom => _sliderZoom;
    public MoveRunSwitcher MoveRunSwitcher => _moveRunSwitcher;
}
