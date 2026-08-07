using System;
using UnityEngine;

public interface IJumpHandler
{
    bool IsJumping { get; }
    public Vector3 VerticalDirection { get; }

    event Action OnJumpEnded;
    event Action OnJumpStarted;

    void StartJump();
    void ForceEndJump();
    void UpdateJumpDirection();
}