using System;
using UnityEngine;
using UnityEngine.UI;

public class AimSwitcher : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    /*[SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _selectedSprite;*/

    private bool _isAim;

    public event Action AimClicked;

    private void OnDestroy() => 
        _button.onClick.RemoveListener(OnClick);

    private void Start() => 
        _button.onClick.AddListener(OnClick);

    public void SetAimState(bool isAim)
    {
        _isAim = isAim;

        /*_image.sprite = _isAim
            ? _selectedSprite
            : _normalSprite;*/
    }

    private void OnClick()
    {
        AimClicked?.Invoke();
    }
}