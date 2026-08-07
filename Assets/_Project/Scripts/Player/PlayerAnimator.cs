using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetRun(bool isRunning)
    {
        animator.SetBool("Run", isRunning);
    }

    public void SetJump(bool isJump)
    {
        animator.SetBool("Jump", isJump);
    }
}
