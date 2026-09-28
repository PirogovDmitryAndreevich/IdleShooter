using TMPro;
using UnityEngine;

public class DamagePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private DamageSlider _damageSlider;

    private DamageManager _damageManager;

    private void OnDestroy()
    {
        _damageManager.DamageChanged -= OnDamageChanged;
    }

    public void Construct(DamageManager damageManager, ILevelManager levelManager)
    {
        _damageManager = damageManager;

        _damageSlider.Construct(levelManager);

        _damageManager.DamageChanged += OnDamageChanged;
        OnDamageChanged(_damageManager.Damage);
    }

    private void OnDamageChanged(float damage)
    {
        _damageText.text = damage.ToString();
    }
}
