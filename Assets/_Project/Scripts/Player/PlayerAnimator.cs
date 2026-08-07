using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;

    public PlayerAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void SetRun(bool isRunning)
    {
        _animator.SetBool("Run", isRunning);
    }

    public void SetJump(bool isJump)
    {
        _animator.SetBool("Jump", isJump);
    }
}
