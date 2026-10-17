using System;

public interface ICrownsManager : IService
{
    event Action CrownChanged;

    void AddCrown(int value);
    int GetCurrentCrowns();
}