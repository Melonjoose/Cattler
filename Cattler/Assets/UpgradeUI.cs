using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;


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

public class UpgradeUI : MonoBehaviour
{
    [Header("Upgrade Menu Options")] // Creates a labeled section in the Inspector
    public GameObject upgradeMenu;
    public GameObject upgradeButtonPrefab; // Prefab for the upgrade option button. //used to instantiate upgrade options in the upgrade menu.
    public GameObject upgradeOptionParent; // Parent transform to hold the upgrade options in the hierarchy.
    public List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    public int pageIndex = 0;
    public int currentPage = 1; // page 1
    public TextMeshProUGUI pageNumberText;
    public List<UpgradePage> upgradePages = new List<UpgradePage>();
    public int maxUpgradePerPage = 4;

    [Header("Description Box Settings")] // Another labeled section
    public DescriptionBox descriptionBox;
    public Transform descriptionBoxDefaultPos;

    public void OpenPage(int page)
    {
        pageIndex = page;              // store 0-based index
        currentPage = page + 1;        // store 1-based display number
        pageNumberText.text = currentPage.ToString();

        // Hide all buttons first
        foreach (UpgradeButton button in upgradeButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Show only the buttons that belong to the selected page
        if (pageIndex >= 0 && pageIndex < upgradePages.Count)
        {
            foreach (UpgradeButton button in upgradePages[pageIndex].upgradeButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }


    public void NextPage()
    {
        if (pageIndex + 1 < upgradePages.Count)
        {
            OpenPage(pageIndex + 1);
        }
    }

    public void PreviousPage()
    {
        if (pageIndex - 1 >= 0)
        {
            OpenPage(pageIndex - 1);
        }
    }


    public void UpdateAllUpgradeButtonsUI()
    {
        foreach(UpgradeButton button in upgradeButtons)
        {
            button.UpdateUpgradeButtonUI();
        }
    }
}
