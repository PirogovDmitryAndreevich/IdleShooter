using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ShootUIEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rect;

    private IPoolService _pool;

    public RectTransform RectTransform =>
        _rect;
    public CanvasGroup CanvasGroup => 
        _canvasGroup;

    public void Construct(IPoolService pool) =>
        _pool = pool;

    public void Show(string value) => 
        _valueText.text = "+" + value;

    public void Hide() =>
        ReturnToPool();

    private void ReturnToPool() =>
        _pool.Return(this);
}