    using UnityEngine;

    [CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public Sprite iconSprite;
        public string upgradeName;
    
        [TextArea(2, 5)] // min lines, max lines
        public string description;
    
        public int price; // cost of the upgrade.
        public int currentLevel;
        public int maxlevel;  //0 means no max level.

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
            currentLevel++;
            price = CalculateNewPrice();
                //for example at level 0, neaning no purchases yet, hence the current totalvalueadded = 0 and the nexttotalvalueadded = 1. 
            currentTotalValueAdded = baseValue + (currentLevel - 1) * valueIncreasePerLevel; //this is to calculate the current total value added to the player after applying the upgrade.
            nextTotalValueAdded = currentTotalValueAdded + baseValue + currentLevel * valueIncreasePerLevel; //this is to calculate the total value that will be added to the player at the next level.
        }

        private int CalculateNewPrice()
        {
            return price * 2; // Example scaling x2 of previous price.
        }
    }
