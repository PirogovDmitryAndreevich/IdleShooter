using System;

public interface IWindowsService : IService
{
    event Action<WindowId> OpeningWindow;

    void OpenWindow(WindowId id);
}