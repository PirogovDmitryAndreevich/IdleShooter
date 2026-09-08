using System;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Action DealDamage;
    public Action AttackEnded;

    /// Called in the animator
    public void InvokeAttackEvent() => 
        DealDamage?.Invoke();

    public void InvokeAttackEndedEvent() => 
        AttackEnded?.Invoke();
}