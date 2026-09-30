using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private RectTransform _healthIcon;
    [SerializeField] private TMP_Text _valueText;

    private PlayerHealth _health;

    public RectTransform HealthIcon => _healthIcon;

    private void OnDestroy()
    {
        if (_health != null)
            _health.HealthChanged -= UpdateHealthView;
    }

    public void Construct(PlayerHealth health)
    {
        _health = health;

        _slider.minValue = 0;
        _slider.maxValue = 1;

        _health.HealthChanged += UpdateHealthView;

        UpdateHealthView();
    }

    private void UpdateHealthView()
    {
        _valueText.text = $"{_health.CurrentHealth} / {_health.MaxHealth}";
        _slider.value = (float)_health.CurrentHealth / _health.MaxHealth;
    }
}