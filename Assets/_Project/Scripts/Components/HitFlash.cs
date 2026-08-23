using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    private static readonly int BaseColor =
        Shader.PropertyToID("_BaseColor");

    [SerializeField] private Color _whiteColor = Color.white;
    [SerializeField] private Color _redColor = new Color(1f, 0.15f, 0.15f);

    [SerializeField] private float _whiteDuration = 0.025f;
    [SerializeField] private float _redDuration = 0.05f;
    [SerializeField] private float _restoreDuration = 0.08f;

    private Renderer[] _renderers;
    private MaterialPropertyBlock _propertyBlock;

    private Color[][] _originalColors;

    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);

        _propertyBlock = new MaterialPropertyBlock();

        _originalColors = new Color[_renderers.Length][];

        for (int rendererIndex = 0; rendererIndex < _renderers.Length; rendererIndex++)
        {
            Material[] materials = _renderers[rendererIndex].sharedMaterials;

            _originalColors[rendererIndex] = new Color[materials.Length];

            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                Material material = materials[materialIndex];

                _originalColors[rendererIndex][materialIndex] =
                    material.HasProperty(BaseColor)
                        ? material.GetColor(BaseColor)
                        : Color.white;
            }
        }
    }

    public void Flash()
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // 1. Белая вспышка
        SetColor(_whiteColor);

        yield return new WaitForSeconds(_whiteDuration);

        // 2. Красная вспышка
        SetColor(_redColor);

        yield return new WaitForSeconds(_redDuration);

        // 3. Возвращение к исходному цвету
        float elapsed = 0f;

        while (elapsed < _restoreDuration)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / _restoreDuration;

            progress = Mathf.SmoothStep(0f, 1f, progress);

            SetLerpColor(_redColor, progress);

            yield return null;
        }

        RestoreColors();

        _flashCoroutine = null;
    }

    private void SetColor(Color color)
    {
        for (int rendererIndex = 0; rendererIndex < _renderers.Length; rendererIndex++)
        {
            Renderer renderer = _renderers[rendererIndex];
            Material[] materials = renderer.sharedMaterials;

            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                if (!materials[materialIndex].HasProperty(BaseColor))
                    continue;

                renderer.GetPropertyBlock(
                    _propertyBlock,
                    materialIndex);

                _propertyBlock.SetColor(
                    BaseColor,
                    color);

                renderer.SetPropertyBlock(
                    _propertyBlock,
                    materialIndex);
            }
        }
    }

    private void SetLerpColor(Color from, float progress)
    {
        for (int rendererIndex = 0; rendererIndex < _renderers.Length; rendererIndex++)
        {
            Renderer renderer = _renderers[rendererIndex];
            Material[] materials = renderer.sharedMaterials;

            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                if (!materials[materialIndex].HasProperty(BaseColor))
                    continue;

                Color originalColor =
                    _originalColors[rendererIndex][materialIndex];

                Color color = Color.Lerp(
                    from,
                    originalColor,
                    progress);

                renderer.GetPropertyBlock(
                    _propertyBlock,
                    materialIndex);

                _propertyBlock.SetColor(
                    BaseColor,
                    color);

                renderer.SetPropertyBlock(
                    _propertyBlock,
                    materialIndex);
            }
        }
    }

    private void RestoreColors()
    {
        for (int rendererIndex = 0; rendererIndex < _renderers.Length; rendererIndex++)
        {
            Renderer renderer = _renderers[rendererIndex];

            for (int materialIndex = 0;
                 materialIndex < _originalColors[rendererIndex].Length;
                 materialIndex++)
            {
                renderer.GetPropertyBlock(
                    _propertyBlock,
                    materialIndex);

                _propertyBlock.SetColor(
                    BaseColor,
                    _originalColors[rendererIndex][materialIndex]);

                renderer.SetPropertyBlock(
                    _propertyBlock,
                    materialIndex);
            }
        }
    }

    private void OnDestroy()
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);
    }
}