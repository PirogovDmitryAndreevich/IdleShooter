using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private Image _character;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _message;
    [SerializeField] private TMP_Text _bonus;
    [SerializeField] private LocalizedStringTable _localizedTable;

    private bool _withButton;
    private LocalizedString _messageLocale;
    private LocalizedString _bonusLocale;

    public event Action ClickedButton;

    private void OnDestroy()
    {
        if (_messageLocale != null)
            _messageLocale.StringChanged -= UpdateMessage;

        if (_bonusLocale != null)
            _bonusLocale.StringChanged -= UpdateBonus;

        _button.onClick.RemoveListener(Hide);
    }

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

        if (_messageLocale != null)
            _messageLocale.StringChanged -= UpdateMessage;

        if (_bonusLocale != null)
            _bonusLocale.StringChanged -= UpdateBonus;

        gameObject.SetActive(false);
    }

    private void SetMessage(MessageStringKey key)
    {
        if (_messageLocale != null)
            _messageLocale.StringChanged -= UpdateMessage;

        _messageLocale = new LocalizedString(
            _localizedTable.TableReference,
            key.ToString());

        _messageLocale.StringChanged += UpdateMessage;
    }

    private void SetBonusString(BonusStringKey key)
    {
        if (_bonusLocale != null)
            _bonusLocale.StringChanged -= UpdateBonus;

        _bonusLocale = new LocalizedString(
            _localizedTable.TableReference,
            key.ToString());

        _bonusLocale.StringChanged += UpdateBonus;
    }

    private void UpdateMessage(string value) => 
        _message.text = value ?? string.Empty;

    private void UpdateBonus(string value) => 
        _bonus.text = value ?? string.Empty;
}
