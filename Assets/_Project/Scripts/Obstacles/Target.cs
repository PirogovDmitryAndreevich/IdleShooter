using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Target Settings")]
    [SerializeField] private float _maxHealth = 10;

    [Header("Debug Settings")]
    [SerializeField] private bool _isViewDebug = false;

    private float _currentHealth;
    private bool _isDied;

    private void Start()
    {
        _currentHealth = _maxHealth;

        _isDied = false;
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, transform.position, Vector3.up);
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _currentHealth = (int)Mathf.Max(_currentHealth - damage, 0);

        if (_isViewDebug)
            Debug.Log($"{gameObject.name} получил {damage} урона. Здоровье {_currentHealth}/{_maxHealth}");

        if (_currentHealth <= 0)
            Died();
    }

    private void Died()
    {
        if(_isDied)
            return;

        _isDied = true;

        if (_isViewDebug)
            Debug.Log($"{gameObject.name} уничтожен/погиб");

        gameObject.SetActive(false);
    }
}