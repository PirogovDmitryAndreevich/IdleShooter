using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetRun(bool isRunning)
    {
        animator.SetBool("run", isRunning);
    }

    public void SetAir(bool isAir)
    {
        animator.SetBool("air", isAir);
    }
}
