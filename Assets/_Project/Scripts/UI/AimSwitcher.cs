using System;
using UnityEngine;
using UnityEngine.UI;

public class AimSwitcher : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    /*[SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _selectedSprite;*/

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

        /*_image.sprite = _isAim
            ? _selectedSprite
            : _normalSprite;*/
    }

    private void OnClick()
    {
        AimClicked?.Invoke();
    }
}