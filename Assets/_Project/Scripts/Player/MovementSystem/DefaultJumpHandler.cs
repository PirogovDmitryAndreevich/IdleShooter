using System;
using UnityEngine;

public class DefaultJumpHandler : IJumpHandler
{
    private bool _isJumping;
    private float _jumpElapsedTime;
    private Vector3 _verticalDirection;
    private PlayerStaticData _data;

    public DefaultJumpHandler(PlayerStaticData data)
    {
        _data = data;
    }

    public bool IsJumping => _isJumping;
    public Vector3 VerticalDirection => _verticalDirection;

    public event Action OnJumpStarted;
    public event Action OnJumpEnded;

    public void StartJump()
    {
        _isJumping = true;
        _jumpElapsedTime = 0;
        OnJumpStarted?.Invoke();
    }

    public void UpdateJumpDirection()
    {
        float directionY = 0;

        if (_isJumping)
        {
            directionY = GetSmoothStep(_data.JumpForce, _data.JumpTime);
            UpdateElapsed(_data.JumpForce, _data.JumpTime);            
        }

        directionY -= _data.Gravity * Time.deltaTime;

        _verticalDirection = Vector3.up * directionY;
    }

    public void ForceEndJump()
    {
        if (!_isJumping) return;

        _isJumping = false;
        _jumpElapsedTime = 0;
        OnJumpEnded?.Invoke();
    }

    private void UpdateElapsed(float jumpForce, float jumpTime)
    {
        _jumpElapsedTime += Time.deltaTime;

        if (_jumpElapsedTime >= jumpTime)
            ForceEndJump();
    }

    private float GetSmoothStep(float jumpForce, float jumpTime) =>
        Mathf.SmoothStep(jumpForce, jumpForce * 0.30f,
            _jumpElapsedTime / jumpTime) * Time.deltaTime;
}