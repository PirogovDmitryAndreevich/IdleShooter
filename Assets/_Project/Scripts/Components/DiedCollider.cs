using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiedCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if (player == null)
        {
            player = other.GetComponentInChildren<IPlayer>();

            if (player == null)
                player = other.GetComponentInParent<IPlayer>();
        }

        if (player != null)
            player.Died();
    }
}