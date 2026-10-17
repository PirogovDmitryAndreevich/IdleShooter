using UnityEngine;

public class LineDraw : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _heightOffset = 0.1f;
    [SerializeField] private float _speed = 1f;

    private Material _material;
    private Transform _character;
    private Transform _target;

    private void Update()
    {
        if (_target == null)
        {
            _line.enabled = false;
            return;
        }

        _material.mainTextureOffset = new Vector2(Time.time * _speed, 0f);

        Vector3 startPosition = _character.position;
        Vector3 endPosition = _target.position;

        startPosition.y += _heightOffset;

        endPosition.y = startPosition.y;

        _line.SetPosition(0, startPosition);
        _line.SetPosition(1, endPosition);
    }

    public void Construct(Transform characterPoint)
    {
        _character = characterPoint;
        _material = _line.material;

        StopDraw();
    }

    public void StartDraw(Transform target)
    {
        Debug.Log("[LineDraw] start draw");

        _target = target;
        _line.enabled = true;
    }

    public void StopDraw()
    {
        _line.enabled = false;
        _target = null;
    }
}
