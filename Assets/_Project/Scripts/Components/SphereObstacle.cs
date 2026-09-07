using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereObstacle : MonoBehaviour
{
    [SerializeField] private DamageableCollider _damageable;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isFinished;

    private float _knockbackMultiplier = 2f;
    private float _maxKnockbackForce = 20f;

    public event Action<SphereObstacle> ReachedFinish;

    private void OnDestroy()
    {
        _damageable.DamageReceived -= TakeDamage;
    }

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

        _damageable.DamageReceived += TakeDamage;
    }

    public void ResetPhysics()
    {
        _isFinished = false;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
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
