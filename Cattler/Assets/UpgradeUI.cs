using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JetBrains.Annotations;


[Serializable]
public class UpgradeOptions //options at upgrade menu that shows the name, amount of time this upgrade has been purchased.
{
    public Upgrade upgrade; //to let this upgradeoption link to the upgrade to get it's info.
    //displays
    public string name; //title of upgrade
    public int level;
    //interactable
    public Button button;
}

[Serializable]
public class UpgradePage
{
    public bool isPageActive = false; //to let the upgrade page know if it is active or not, so that it can show or hide the upgrade options that belong to it.
    public int pageNumber; //to let this upgradepage link to the page number to know which page it is.
    public List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
}

[Serializable]
public class DesriptionBox
{
    public Upgrade upgrade; //to let this descriptionbox link to the upgrade to get it's info.

    public GameObject descriptionBox; //the description box gameobject to show the description of the upgrade when player clicks on the upgrade option.

    public string upgradeName;

    [TextArea(2, 5)] // min lines, max lines
    public string description;

    public int price; // cost of the upgrade.
    public int currentlevel;
    public int maxlevel;  //0 means no max level.

    public float currentTotalValueAdded; //this is to show the current totalvalue that this upgrade is currently being added to the player.
    public float nextTotalValueAdded; //this is to show the totalvalue that this upgrade will be adding to the player at the next level.

    public float baseValue; // value of the upgrade at level 1, this is the value that will be increased by valueIncreasePerLevel every level.
    public float valueIncreasePerLevel; // to increase the value of every level. making the upgrade scale. if I want it flat, set this to 0.

}

public class UpgradeUI : MonoBehaviour
{
    [Header("Upgrade Menu Options")] // Creates a labeled section in the Inspector
    public GameObject upgradeMenu;
    public GameObject upgradeButtonPrefab; // Prefab for the upgrade option button. //used to instantiate upgrade options in the upgrade menu.
    public GameObject upgradeOptionParent; // Parent transform to hold the upgrade options in the hierarchy.
    public List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    public int currentPage = 0; // page 1
    public List<UpgradePage> upgradePages = new List<UpgradePage>();
    public int maxUpgradePerPage = 4;

    [Header("Description Box Settings")] // Another labeled section
    public DesriptionBox descriptionBox;
    public Transform descriptionBoxDefaultPos;

    void Start()
    {
        descriptionBox.descriptionBox.SetActive(false); //hide the description box at the start of the game.
    }

    public void OpenPage(int page)
    {
        currentPage = page;

        // Hide all buttons first
        foreach (UpgradeButton button in upgradeButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Show only the buttons that belong to the selected page
        if (page >= 0 && page < upgradePages.Count)
        {
            foreach (UpgradeButton button in upgradePages[page].upgradeButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    public void NextPage()
    {
        if (currentPage + 1 < upgradePages.Count)
        {
            OpenPage(currentPage + 1);
        }
    }

    public void PreviousPage()
    {
        if (currentPage - 1 >= 0)
        {
            OpenPage(currentPage - 1);
        }
    }

}
