using System.Collections.Generic;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public UpgradeUI upgradeUI;

    public Upgrade[] upgrades; // all available upgrades

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initialize all upgrade options at start
        foreach (var upgrade in upgrades)
        {
            InitializeUpgradeUIOption(upgrade); //create buttons
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
            buttonIndex++;

            upgradeUI.upgradePages.Add(newPage);
        }
    }

}
