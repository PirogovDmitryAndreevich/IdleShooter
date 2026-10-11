using System;
using UnityEngine;

public class EnemyStepAnimationEvent : MonoBehaviour
{
    public Action Step;

    public void InvokeStep() =>
        Step?.Invoke();
}