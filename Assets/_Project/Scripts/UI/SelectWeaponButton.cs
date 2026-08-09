using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private Hud _hud;
    private WeaponType _weaponType;
    private bool _isSelected;

    private void OnDestroy() =>
        _button.onClick.RemoveListener(OnClick);

    public void Construct(Hud hud, IWeapon weapon)
    {
        _hud = hud;
        _weaponType = weapon.Type;
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        WeaponType toSent;

        if (_isSelected)
        {
            toSent = WeaponType.Deselect;
            _isSelected = false;
        }
        else
        {
            _isSelected = true;
            toSent = _weaponType;
        }

        _hud.SwitchWeapon(toSent);
    }
}