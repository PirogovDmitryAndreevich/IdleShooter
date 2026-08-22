using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerTriggerObserver : MonoBehaviour
{
    public event Action TriggerEnter;
    public event Action<CharacterController> PlayerEntered;
    public event Action PlayerExited;

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
        {
            TriggerEnter?.Invoke();

            if (other.TryGetComponent(out CharacterController cc))
            {
                PlayerEntered?.Invoke(cc);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterController cc))
            PlayerExited?.Invoke();
    }
}