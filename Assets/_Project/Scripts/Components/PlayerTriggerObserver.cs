using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerTriggerObserver : MonoBehaviour
{
    public event Action TriggerEnter;
    public event Action<Transform> TargetEnter;
    public event Action<CharacterController> ControllerEntered;
    public event Action<IPassiveShooterInteractor> PassiveShooterEntered;
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
                ControllerEntered?.Invoke(cc);        
            
            if(other.TryGetComponent(out Transform transform))
                TargetEnter?.Invoke(transform);

            if(other.TryGetComponent(out IPassiveShooterInteractor passiveveShooter))
                PassiveShooterEntered?.Invoke(passiveveShooter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterController cc))
            PlayerExited?.Invoke();
    }
}