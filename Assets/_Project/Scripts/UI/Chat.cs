using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private Image _character;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _message;
    [SerializeField] private TMP_Text _bonus;
    [SerializeField] private LocalizedStringTable _localizedTable;

    private bool _withButton;

    public event Action ClickedButton;

    private void OnDestroy() =>
        _button.onClick.RemoveListener(Hide);

    public void Show(Sprite character, MessageStringKey messageKey, BonusStringKey bonusKey, bool withButton = true)
    {
        _character.sprite = character;
        _withButton = withButton;

        gameObject.SetActive(true);

        SetMessage(messageKey);
        SetBonusString(bonusKey);

        _button.gameObject.SetActive(_withButton);

        if (withButton)
            _button.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        if (_withButton)
        {
            _button.onClick.RemoveListener(Hide);
            _button.gameObject.SetActive(false);
            _withButton = false;

            ClickedButton?.Invoke();
        }

        gameObject.SetActive(false);
    }

    private void SetMessage(MessageStringKey key)
    {
        LocalizedString localizedString = new LocalizedString(
        _localizedTable.TableReference,
        key.ToString());

        _message.text = localizedString.GetLocalizedString() ?? string.Empty;
    }

    private void SetBonusString(BonusStringKey key)
    {
        LocalizedString localizedString = new LocalizedString(
        _localizedTable.TableReference,
        key.ToString());

        _bonus.text = localizedString.GetLocalizedString() ?? string.Empty;
    }
}
