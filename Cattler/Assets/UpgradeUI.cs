using UnityEngine;

public class UpgradeOptions //options at upgrade menu that shows the name, amount of time this upgrade has been purchased.
{
    public Upgrade upgrade; //to let this upgradeoption link to the upgrade to get it's info.
 
    public string name; //title of upgrade
    public int level;
}

public class DesriptionBox
{
    public Upgrade upgrade; //to let this descriptionbox link to the upgrade to get it's info.

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
    public GameObject upgradeMenu;
    public UpgradeOptions[] upgradeOptions;

    public GameObject descriptionBox;
    public Transform descriptionBoxDefaultPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
