using System;

public class WindowsService : IWindowsService
{
    public event Action<WindowId> OpeningWindow;

    public void OpenWindow(WindowId id) =>
        OpeningWindow?.Invoke(id);
}
