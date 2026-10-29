using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _valueText;

    private Health _health;

    private void OnDestroy()
    {
        if (_health != null)
            _health.HealthChanged -= UpdateHealthView;
    }

    public void Construct(Health health)
    {
        _health = health;

        _slider.minValue = 0;
        _slider.maxValue = 1;

        _health.HealthChanged += UpdateHealthView;

        UpdateHealthView();
    }

    private void UpdateHealthView()
    {
        _valueText.text = $"{NumberFormatter.NumberAsString(_health.CurrentHealth)} /" +
            $" {NumberFormatter.NumberAsString(_health.MaxHealth)}";

        _slider.value = (float)_health.CurrentHealth / _health.MaxHealth;
    }
}