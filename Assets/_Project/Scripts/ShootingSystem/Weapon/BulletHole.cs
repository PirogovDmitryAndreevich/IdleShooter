using System.Collections;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    private readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _fadeDuration = 0.5f;

    private IPoolService _pool;
    private MaterialPropertyBlock _propertyBlock;
    private Color _color = Color.black;

    private Coroutine _coroutine;

    public void Construct(IPoolService pool)
    {
        _pool = pool;
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void Show(Vector3 point, Vector3 normal)
    {
        transform.position = point + normal * 0.01f;
        transform.rotation = Quaternion.LookRotation(-normal);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        SetAlpha(1f);

        yield return new WaitForSeconds(_lifeTime);

        float time = 0f;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = 1f - time / _fadeDuration;
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0f);

        _pool.Return(this);
    }

    private void SetAlpha(float alpha)
    {
        _color.a = alpha;

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(BaseColor, _color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
