public class GiftService : IGiftService
{
    private readonly ILevelManager _levelManager;

    public GiftService(ILevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    public void AddGift(GiftType type, float value)
    {
        switch (type)
        {
            case GiftType.first:
                AddScoreGift(value);
                break;
            case GiftType.second:
                AddScoreGift(value);
                break;
            case GiftType.third:
                AddMultiplyGift(value);
                break;
            case GiftType.fourth:
                AddMultiplyGift(value);
                break;
            case GiftType.fifth:
                AddWinsGift((int)value);
                break;
        }
    }

    private void AddScoreGift(float value) =>
        _levelManager.AddScore(value);

    private void AddMultiplyGift(float multilayer)
    {

    }

    private void AddWinsGift(int value) =>
        _levelManager.ChangeWins(value);
}