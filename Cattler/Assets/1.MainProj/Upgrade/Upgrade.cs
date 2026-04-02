using System;
using UnityEngine;

public enum UpgradeType { AttackPower, AttackSpeed, Health, MovementSpeed, Range,InventorySpace, Others }
[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade/Upgrade")]
public class Upgrade : ScriptableObject
{
    public Sprite iconSprite;
    public string upgradeName;
    public UpgradeType upgradeType;
    [TextArea(2, 5)] // min lines, max lines
    public string description;
    
    public int price; // cost of the upgrade.
    public int maxPrice; //the limit how expensive this upgrade can be.if 0, then unlimited.
    public int currentLevel;
    public int maxlevel;  //0 means no max level.

    public string summaryUpgradeText; // used as $"currentotalvalueadded" + $"summaryUpgradeText" to get something like +1 MaxHP to newly summoned cats.

    public float currentTotalValueAdded; //this is to show the current totalvalue that this upgrade is currently being added to the player.
    public float nextTotalValueAdded; //this is to show the totalvalue that this upgrade will be adding to the player at the next level.

    public float baseValue; // value of the upgrade at level 1, this is the value that will be increased by valueIncreasePerLevel every level.
    public float valueIncreasePerLevel; // to increase the value of every level. making the upgrade scale. if I want it flat, set this to 0.

    public bool CanAfford(int playerCurrency)
    {
        return playerCurrency >= price; //return true if player currency is greater than price.
    }

    public void ApplyUpgrade()
    {
        // Increase the level first
        currentLevel++;
        price = CalculateNewPrice();

        // Current total value = baseValue + (currentLevel - 1) * increment
        currentTotalValueAdded = baseValue + (currentLevel - 1) * valueIncreasePerLevel;

        // Next total value = baseValue + currentLevel * increment
        nextTotalValueAdded = baseValue + currentLevel * valueIncreasePerLevel;
    }


    private int CalculateNewPrice()
    {
        // Scale price by 1.5x
        int newPrice = Mathf.RoundToInt(price * 1.5f);

        // If maxPrice == 0, treat as unlimited
        if (maxPrice == 0)
        {
            return newPrice;
        }

        // Otherwise clamp to maxPrice
        return Mathf.Min(newPrice, maxPrice);
    }

    public Upgrade Clone()
    {
        Upgrade clone = ScriptableObject.CreateInstance<Upgrade>();
        clone.iconSprite = iconSprite;
        clone.upgradeName = upgradeName;
        clone.upgradeType = upgradeType;
        clone.description = description;
        clone.price = price;
        clone.currentLevel = currentLevel;
        clone.maxlevel = maxlevel;
        clone.summaryUpgradeText = summaryUpgradeText;
        clone.currentTotalValueAdded = currentTotalValueAdded;
        clone.nextTotalValueAdded = nextTotalValueAdded;
        clone.baseValue = baseValue;
        clone.valueIncreasePerLevel = valueIncreasePerLevel;
        return clone;
    }


}
