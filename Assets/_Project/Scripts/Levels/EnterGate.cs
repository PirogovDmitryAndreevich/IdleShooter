using UnityEngine;
using UnityEngine.UI;

public class EnterGate : MonoBehaviour
{
    [SerializeField] private Image _lock;
    [SerializeField] private Collider _collider;

    private void Start() => 
        Lock();

    public void Lock()
    {
        if (_lock == null || _collider == null)
            return;

        _lock.gameObject.SetActive(true);
        _collider.isTrigger = false;
    }

    public void Open()
    {
        if (_lock == null || _collider == null)
            return;

        _lock.gameObject.SetActive(false);
        _collider.isTrigger = true;
    }
}