using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeDamageCollider : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private float _damage;

    private void OnTriggerEnter(Collider other)
    {
        IPlayer player = GetPlayer(other);

        if (player == null)
            return;

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        Vector3 direction = (hitPoint - transform.position).normalized;

        Vector3 hitNormal = -direction;

        player.TakeDamage(_damage, hitPoint, hitNormal);
    }

    public void Construct(float damage) => 
        _damage = damage;

    public void Enable(bool isEnable) => 
        _collider.enabled = isEnable;

    private IPlayer GetPlayer(Collider collider)
    {
        IPlayer player = collider.GetComponent<IPlayer>();

        if (player == null)
        {
            player = collider.GetComponentInChildren<IPlayer>();

            if (player == null)
                player = collider.GetComponentInParent<IPlayer>();
        }

        return player;
    }
}