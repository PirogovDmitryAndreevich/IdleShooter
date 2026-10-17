using System;

public class CrownsManager : ICrownsManager
{
    private readonly IProgressService _progressService;

    public event Action CrownChanged;

    public CrownsManager(IProgressService progressService) =>
        _progressService = progressService;

    public void AddCrown(int value)
    {
        _progressService.Data.PlayerData.Crowns += value;
        CrownChanged?.Invoke();
    }

    public int GetCurrentCrowns() =>
        _progressService.Data.PlayerData.Crowns;
}