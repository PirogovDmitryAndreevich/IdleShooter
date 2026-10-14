using System.Collections.Generic;
using System.Linq;

public class WeaponBuyController : IWeaponBuyController
{
    private readonly ILevelManager _levelManager;
    private readonly IStaticDataService _staticDataService;
    private readonly IWeaponDataService _weaponDataService;
    private readonly Dictionary<WeaponType, WeaponStaticData> _weapons;

    public WeaponBuyController(ILevelManager levelManager,
        IStaticDataService staticDataService,
        IWeaponDataService weaponDataService)
    {
        _levelManager = levelManager;
        _staticDataService = staticDataService;
        _weaponDataService = weaponDataService;

        GameStaticData gameData = _staticDataService.GetGameData();

        _weapons = gameData.WeaponsStaticData
            .ToDictionary(x => x.Type, x => x);
    }

    public void BuyWeapon(WeaponType type)
    {
        if (!_weapons.TryGetValue(type, out WeaponStaticData weapon))
            return;

        if (_levelManager.Wins < weapon.Price)
            return;

        _levelManager.SpendWins(weapon.Price);
        _weaponDataService.AddNewWeapon(type);
    }
}