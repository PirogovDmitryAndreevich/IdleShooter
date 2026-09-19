using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereObstacle : MonoBehaviour
{
    [SerializeField] private DamageableCollider Damageable;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isFinished;

    private float _knockbackMultiplier;
    private float _maxKnockbackForce;

    public event Action<SphereObstacle> ReachedFinish;

    protected Rigidbody Rigidbody => _rigidbody;

    private void OnDestroy() => 
        Damageable.DamageReceived -= TakeDamage;

    private void OnCollisionEnter(Collision collision)
    {
        IPlayer player = collision.collider.GetComponent<IPlayer>();

        if (player == null)
        {
            player = collision.collider.GetComponentInChildren<IPlayer>();

            if (player == null)
                player = collision.collider.GetComponentInParent<IPlayer>();
        }

        if (player != null)
            player.Died();
    }

    public void Construct(float knockbackMultiplier, float maxKnockbackForce)
    {
        _knockbackMultiplier = knockbackMultiplier;
        _maxKnockbackForce = maxKnockbackForce;

        Damageable.DamageReceived += TakeDamage;
    }

    public void ResetPhysics()
    {
        _isFinished = false;

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (damage <= 0f)
            return;

        float force = Mathf.Min(Mathf.Sqrt(damage) * _knockbackMultiplier, _maxKnockbackForce);

        Rigidbody.AddForce(
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
