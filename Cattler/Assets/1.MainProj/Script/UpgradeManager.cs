using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public UpgradeUI upgradeUI;
    public DescriptionBox descBox;

    public Upgrade[] upgradesLibrary; // all available upgrades library

    [SerializeReference]
    public List<Upgrade> upgrades = new List<Upgrade>(); //instantiate or copy the data to be changed/altered ingame

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        upgrades.Clear();
        // Initialize all upgrade options at start
        foreach (var upgrade in upgradesLibrary)
        {
            // Create a new instance (so we don’t modify the library directly)
            Upgrade runtimeUpgrade = Instantiate(upgrade);
            runtimeUpgrade.name = $"{upgrade.name} RT";
            upgrades.Add(runtimeUpgrade);

            InitializeUpgradeUIOption(runtimeUpgrade);
        }
        //check how many buttons are there.
        // If there are more than maxUpgradePerPage, we need to create new pages and assign buttons to them.
        AssignButtonsToPage();
        upgradeUI.OpenPage(0);
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

    public void InitializeUpgradeUIOption(Upgrade upgrade)
    {
        // Instantiate a button prefab under the upgrade menu
        GameObject buttonObj = Instantiate(upgradeUI.upgradeButtonPrefab, upgradeUI.upgradeOptionParent.transform);

        // Get the UpgradeButton component
        UpgradeButton upgradeButton = buttonObj.GetComponent<UpgradeButton>();

        upgradeButton.name = $"{upgrade.name} Button"; // Set the name of the button in the hierarchy for clarity
        // Link the upgrade data
        upgradeButton.upgrade = upgrade;

        // Update button text (assuming you have a Text component on the prefab)
        Text buttonText = buttonObj.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = $"{upgrade.name} (Lv {upgrade.currentLevel})";
        }

        upgradeButton.InitializeButton(); // Initialize the button (set up listeners, etc.)
        upgradeButton.UpdateUpgradeButtonUI(); // Update the button UI with the upgrade info

        upgradeUI.upgradeButtons.Add(upgradeButton); // Add to the array for reference

    }

    public void AssignButtonsToPage()
    {
        if (upgradeUI.upgradeButtons == null || upgradeUI.upgradeButtons.Count == 0) return;

        // Clear any existing pages before reassigning
        upgradeUI.upgradePages.Clear();

        int totalButtons = upgradeUI.upgradeButtons.Count; //currently 6
        int pageCount = Mathf.CeilToInt((float)totalButtons / upgradeUI.maxUpgradePerPage); // check how many page needed.
        //based on my maxUpgdradePerPage == 4. it should be 2page. holding 4 and 2 upgrades.
        int buttonIndex = 0;

        for (int i = 0; i < pageCount; i++) //loop through each page.
        {
            UpgradePage newPage = new UpgradePage
            {
                pageNumber = i,
                isPageActive = (i == 0)
            };

            for (int j = 0; j < upgradeUI.maxUpgradePerPage && buttonIndex < totalButtons; j++)
            {
                newPage.upgradeButtons.Add(upgradeUI.upgradeButtons[buttonIndex]);
                buttonIndex++;
            }

            upgradeUI.upgradePages.Add(newPage);
        }
    }

    public void PurchaseUpgrade(int inkCost) // logic when purchase happens
    {
        Currency.instance.AddInk(-inkCost);


        descBox.upgrade.ApplyUpgrade(); // update stats of the upgrade 
        UpgradeInventorySpace(); //if the upgrade type is inventoryspace.
        descBox.UpdateDescriptionBox(descBox.upgrade);
    }

    public void ApplyUpgradesToNewCat(CatUnit cat) //affects inventory.
    {
        Debug.Log($"Applying upgrades to new cat: {cat.name}");

        foreach (var upgrade in upgrades)
        {
            if (upgrade.currentTotalValueAdded > 0)
            {
                switch (upgrade.upgradeType)
                {
                    case UpgradeType.AttackPower:
                        cat.runtimeData.attackPower += (int)upgrade.currentTotalValueAdded;
                        Debug.Log($"Attack Power +{upgrade.currentTotalValueAdded} > {cat.runtimeData.attackPower}");
                        break;

                    case UpgradeType.AttackSpeed:
                        cat.runtimeData.attackSpeed += upgrade.currentTotalValueAdded;
                        Debug.Log($"Attack Speed +{upgrade.currentTotalValueAdded} > {cat.runtimeData.attackSpeed}");
                        break;

                    case UpgradeType.Range:
                        cat.runtimeData.attackRange += upgrade.currentTotalValueAdded;
                        Debug.Log($"Attack Range +{upgrade.currentTotalValueAdded} > {cat.runtimeData.attackRange}");
                        break;

                    case UpgradeType.Health:
                        cat.runtimeData.maxHealth += (int)upgrade.currentTotalValueAdded;
                        cat.runtimeData.currentHealth += (int)upgrade.currentTotalValueAdded;
                        Debug.Log($"Health +{upgrade.currentTotalValueAdded} > {cat.runtimeData.currentHealth}/{cat.runtimeData.maxHealth}");
                        break;

                    case UpgradeType.MovementSpeed:
                        cat.runtimeData.movementSpeed += upgrade.currentTotalValueAdded;
                        Debug.Log($"Movement Speed +{upgrade.currentTotalValueAdded} > {cat.runtimeData.movementSpeed}");
                        break;
                }
            }
        }
    }

    public void UpgradeInventorySpace()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.currentTotalValueAdded > 0)
            {
                switch (upgrade.upgradeType)
                {
                    case UpgradeType.InventorySpace:
                        Debug.Log("Upgrade Inventory");
                        Inventory.instance.IncreaseCapacity(1);
                        break;

                }
            }
        }
    }
}

