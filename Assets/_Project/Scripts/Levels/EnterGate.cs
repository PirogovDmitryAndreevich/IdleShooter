using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterGate : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _recommendedLevel;
    [SerializeField] private Image _lock;
    [SerializeField] private Collider _collider;

    public void Construct(LevelsStaticData data)
    {
        _levelText.text = ((int)data.Type).ToString();

        _recommendedLevel.text = data.Recommended.ToString();

        Lock();
    }

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