using System;
using UnityEngine;
using UnityEngine.UI;

public class AimSwitcher : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _viewSprite;
    [SerializeField] private Sprite _aimSprite;

    public event Action AimClicked;

    public bool IsAim { get; private set; }

    private void OnDestroy() => 
        _button.onClick.RemoveListener(OnClick);

    private void Start()
    {
        IsAim = false;

        _button.onClick.AddListener(OnClick);
    }

    public void SetAimState(bool isAim)
    {
        IsAim = isAim;

        _image.sprite = IsAim
            ? _aimSprite
            : _viewSprite;
    }

    private void OnClick()
    {
        AimClicked?.Invoke();
    }
}