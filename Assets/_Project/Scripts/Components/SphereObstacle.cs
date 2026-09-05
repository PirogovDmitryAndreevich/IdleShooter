using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereObstacle : MonoBehaviour, IDamageable
{
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isFinished;

    private float _knockbackMultiplier = 2f;
    private float _maxKnockbackForce = 20f;

    public event Action<SphereObstacle> ReachedFinish;

    public void Construct(float knockbackMultiplier, float maxKnockbackForce)
    {
        _knockbackMultiplier = knockbackMultiplier;
        _maxKnockbackForce = maxKnockbackForce;
    }

    public void ResetPhysics()
    {
        _isFinished = false;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void TakeDamage(float damage)
    {

    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (damage <= 0f)
            return;

        float force = Mathf.Min(Mathf.Sqrt(damage) * _knockbackMultiplier, _maxKnockbackForce);

        _rigidbody.AddForce(
            -hitNormal.normalized * force,
            ForceMode.Impulse);
    }

    public void ReachFinish()
    {
        if (_isFinished)
            return;

        _isFinished = true;

        ReachedFinish?.Invoke(this);
    }
}
