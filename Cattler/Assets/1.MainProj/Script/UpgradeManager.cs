using System.Diagnostics;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //in-charge of upgrades.
    public static UpgradeManager instance;

    public Upgrade[] upgrades; //to list out all the upgrades that the player can purchase, this is to let upgradeUI intialize how many upgrade options and add them into the upgrade menu.

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TryUpgrade(string upgradeName, int playerCurrency)
    {
        Upgrade upgrade = GetUpgradeByName(upgradeName);
        if (upgrade != null && upgrade.CanAfford(playerCurrency))
        {
            upgrade.ApplyUpgrade();
            // Deduct currency from player here
            return true;
        }
        return false;
    }

    private Upgrade GetUpgradeByName(string name)
    {
        foreach (var u in upgrades)
        {
            if (u.name == name) return u;
        }
        return null;
    }

}
