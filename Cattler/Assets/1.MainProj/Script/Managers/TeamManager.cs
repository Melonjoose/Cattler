using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Xml;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int currentTeamSize = 0;
    public int availableTeamSlots = 3; // can expand up to 5
    public int maxTeamSlots = 5;
    public List<ContainerDetector> catContainers = new List<ContainerDetector>(); 
    public List<CatUnit> cats = new List<CatUnit>(); 

    public GameObject playerTeamGO;

    public GameObject catTemplatePrefab;

    private void Awake()
    {
        instance = this;
        InitializeContainers();
    }

    public void AddCatToWorld(CatUnit newlyAddedCat, SnappableLocation slot)
    {
        if (currentTeamSize >= availableTeamSlots)
        {
            Debug.Log("No free team slots available!");
            return;
        }

        CatUnit newCatUnit;

        if (newlyAddedCat.catGO == null)  //if first time added to world.
        {
            GameObject newCatGO = Instantiate(catTemplatePrefab);
            newCatGO.name = newlyAddedCat.runtimeData.template.itemName;

            currentTeamSize++;

            newCatUnit = newCatGO.GetComponent<CatUnit>();
            newCatUnit.runtimeData = newlyAddedCat.runtimeData;
            SpriteRenderer catSprite = newCatUnit.GetComponent<SpriteRenderer>();
            catSprite.sprite = newCatUnit.runtimeData.template.icon;

            newlyAddedCat.catGO = newCatUnit.gameObject;

            var catGO = newlyAddedCat.catGO;
            var catHat = newlyAddedCat.hat;
            var catWeaponL = newlyAddedCat.weaponL;
            var catWeaponR = newlyAddedCat.weaponR;

            //if newlyAddedCat has item equipped. show item on catGO.
            if (catHat != null)
            {

            }
            if (catWeaponL != null)
            {
                EquipItem(catWeaponL , catGO);
            }
            if (catWeaponR != null)
            {

            }

            if (!cats.Contains(newCatUnit))
            {
                cats.Add(newCatUnit);
            }
            
            AddCatToTeam(newCatUnit, slot);
        }

        else // readded into the world.
        {
            CatUnit existingCatUnit = newlyAddedCat.catGO.GetComponent<CatUnit>();
            existingCatUnit.gameObject.SetActive(true);
            if (!cats.Contains(existingCatUnit))
            {
                cats.Add(existingCatUnit);
            }

            AddCatToTeam(existingCatUnit, slot);
        }
    }

    public void AddCatToTeam(CatUnit cat , SnappableLocation slot)  // Team is not empty & ONLY to be added into the world when battle begin 
    {


        CatMovement catMovement = cat.GetComponent<CatMovement>();
        if (catMovement == null)
        {
            Debug.LogError("The provided GameObject does not have a CatMovement component.");
            return;
        }

        //int slotIndex = emptyContainer.containerIndex;
        int slotIndex = slot.SlotIndex;
        cat.transform.position = catContainers[slotIndex].transform.position;
        catMovement.MoveToDesignatedLocation(slotIndex);
        catContainers[slot.SlotIndex].occupyingCat = cat;


        // Move cat to container position

        // Optional: Parent under PlayerTeam
        if (playerTeamGO != null)
            cat.transform.SetParent(playerTeamGO.transform);

        CatIconUI.instance.LinkCatToIcon(slotIndex , cat);

        return;
    }

    public void RemoveCatFromWorld(CatUnit cat, SnappableLocation slot)
    {
        if (cat == null || cat.catGO == null)
        {
            Debug.LogWarning("Attempted to remove a null or uninitialized cat.");
            return;
        }

        cats.Remove(cat);
        GameObject worldCat = cat.catGO;
        CatUnit worldCatUnit = worldCat.GetComponent<CatUnit>();

        if (worldCatUnit == null)
        {
            Debug.LogWarning("Cat GameObject does not contain a CatUnit component.");
            return;
        }

        CatIconUI.instance?.UnlinkCatFromIcon(worldCatUnit);

        worldCat.SetActive(false); // Hide cat in world
        cats.Remove(worldCatUnit);          // Remove from the list
        catContainers[slot.SlotIndex].occupyingCat = null;
    }


    void InitializeContainers()
    {
        // Find the parent object that holds the containers
        GameObject playerTeam = GameObject.Find("PlayerTeam");
        if (playerTeam == null)
        {
            Debug.LogError("PlayerTeam GameObject not found in scene!");
            return;
        }

        // Clear and repopulate list automatically
        catContainers.Clear();
        foreach (Transform child in playerTeam.transform)
        {
            ContainerDetector container = child.GetComponent<ContainerDetector>();
            if (container != null)
            {
                catContainers.Add(container);
            }
        }

        // Assign indices to each container
        for (int i = 0; i < catContainers.Count; i++)
        {
            catContainers[i].containerIndex = i;
        }

        Debug.Log($"Initialized {catContainers.Count} cat containers under PlayerTeam.");

        //unlink cat UI.
    }

    ContainerDetector FindEmptyContainer()
    {
        foreach(var container in catContainers)
        {
            if (!container.IsOccupied)
            {
                return container;
            }
        }
        return null;
    }


    void EquipItem(Item item, GameObject worldCat)
    {
        if (item == null || item.runtimeData?.template?.icon == null)
        {
            Debug.LogWarning("Invalid item or missing icon.");
            return;
        }

        Sprite newIcon = item.runtimeData.template.icon;

        // Hat Slot
        Transform hatSlot = worldCat.transform.Find("T_HatSlot");
        if (hatSlot != null)
        {
            Transform hat = hatSlot.Find("Hat");
            if (hat != null)
            {
                //instantiate prefab under hatslot
            }
        }

        // Left Weapon Slot
        Transform leftWeaponSlot = worldCat.transform.Find("L_WeaponSlot");
        if (leftWeaponSlot != null)
        {
            Transform weapon = leftWeaponSlot.Find("Weapon");
            if (weapon != null)
            {
                //instantiate prefab under leftweaponslot
                Instantiate(item.runtimeData.template.prefab, leftWeaponSlot);
            }
        }

        // Right Weapon Slot
        Transform rightWeaponSlot = worldCat.transform.Find("R_WeaponSlot");
        if (rightWeaponSlot != null)
        {
            Transform weapon = rightWeaponSlot.Find("Weapon");
            if (weapon != null)
            {
                SpriteRenderer weaponRenderer = weapon.GetComponent<SpriteRenderer>();
                if (weaponRenderer != null)
                {
                    weaponRenderer.sprite = newIcon;
                }
            }
        }
    }
}
