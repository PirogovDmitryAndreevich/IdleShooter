using System;
using System.Collections;

public class LevelController : ILevelController
{
    private readonly LevelRegister _register;
    private readonly LevelWindow _window;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IWarpService _warpService;

    private LevelType? _currentLevel;
    private LevelType _requestedLevel;

    private bool _isSwitching;
    private bool _shouldTeleport;

    public LevelController(
        ILevelLoader levelLoader,
        IWarpService warpStarter,
        ICoroutineRunner coroutineRunner)
    {
        LevelSequence sequence = new LevelSequence();
        _register = new LevelRegister(levelLoader, sequence);
        _window = new LevelWindow(_register, sequence);

        _coroutineRunner = coroutineRunner;
        _warpService = warpStarter;

        _warpService.TeleportTo += WarpToLevel;

        _register.LevelTransitionEntered += SwitchLevel;
    }

    public void SwitchLevel(LevelType type) =>
        RequestLevel(type, false);

    public void WarpToLevel(LevelType type) =>
        RequestLevel(type, true);

    private void RequestLevel(LevelType type, bool shouldTeleport)
    {
        _requestedLevel = type;
        _shouldTeleport = shouldTeleport;

        if (_isSwitching)
            return;

        if (_currentLevel == type)
        {
            if (_shouldTeleport)
            {
                _warpService.TeleportingTo(type);
                _shouldTeleport = false;
            }

            return;
        }

        _coroutineRunner.StartCoroutine(
            SwitchLevelRoutine());
    }

    private IEnumerator SwitchLevelRoutine()
    {
        _isSwitching = true;

        while (_currentLevel != _requestedLevel)
        {
            LevelType target = _requestedLevel;

            yield return _register.EnsureWindow(target);

            _currentLevel = target;

            _window.Update(target);

            if (_shouldTeleport)
            {
                UnityEngine.Debug.Log($"WARP TELEPORT: {target}");

                _warpService.TeleportingTo(target);

                _shouldTeleport = false;
            }
        }

        _isSwitching = false;
    }

    public void Dispose()
    {
        _warpService.TeleportTo -=
            WarpToLevel;

        _register.LevelTransitionEntered -=
            SwitchLevel;

        _register.Dispose();
    }
}