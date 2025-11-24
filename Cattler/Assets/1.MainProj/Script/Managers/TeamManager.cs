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
            GameObject newCatGO = Instantiate(catTemplatePrefab);  //new  cat gameobject in the world.
            newCatGO.name = newlyAddedCat.runtimeData.template.itemName;

            currentTeamSize++;

            newCatUnit = newCatGO.GetComponent<CatUnit>(); //cat gameobject's catunit component.
            newCatUnit.runtimeData = newlyAddedCat.runtimeData;
            SpriteRenderer catSprite = newCatUnit.GetComponent<SpriteRenderer>();
            catSprite.sprite = newCatUnit.runtimeData.template.icon;

            newlyAddedCat.catGO = newCatUnit.gameObject;

            newCatUnit.catGO = newCatUnit.gameObject;

            newCatUnit.weaponL = newlyAddedCat.weaponL;
            newCatUnit.weaponR = newlyAddedCat.weaponR;
            newCatUnit.hat = newlyAddedCat.hat;

            Debug.Log("AddCatToWorld");
            //if newlyAddedCat has item equipped. show item on catGO.
            if (newCatUnit.hat != null)
            {
                EquipItem(newCatUnit.hat, newCatUnit, "Hat");
            }
            if (newCatUnit.weaponL != null)
            {
                Debug.Log("AddCatToWorldWithLeftWeapon");
                EquipItem(newCatUnit.weaponL, newCatUnit , "L_Weapon");
            }
            if (newCatUnit.weaponR != null)
            {
                EquipItem(newCatUnit.weaponR, newCatUnit, "R_Weapon");
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

            existingCatUnit.weaponL = newlyAddedCat.weaponL;
            existingCatUnit.weaponR = newlyAddedCat.weaponR;
            existingCatUnit.hat = newlyAddedCat.hat;

            Debug.Log("AddCatToWorld");
            //if newlyAddedCat has item equipped. show item on catGO.
            if (existingCatUnit.hat != null)
            {
                EquipItem(existingCatUnit.hat, existingCatUnit, "Hat");
            }
            if (existingCatUnit.weaponL != null)
            {
                Debug.Log("AddCatToWorldWithLeftWeapon");
                EquipItem(existingCatUnit.weaponL, existingCatUnit, "L_Weapon");
            }
            if (existingCatUnit.weaponR != null)
            {
                EquipItem(existingCatUnit.weaponR, existingCatUnit, "R_Weapon");
            }

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


    void EquipItem(Item item, CatUnit worldCat, string slotName)
    {
        Debug.Log("additemtocat");
        if (item == null || item.runtimeData?.template?.icon == null)
        {
            Debug.LogWarning("Invalid item or missing icon.");
            return;
        }

        Sprite newIcon = item.runtimeData.template.icon;

        // Hat Slot
        if (slotName == "Hat")
        {
            Transform hatSlot = worldCat.transform.Find("T_HatSlot");
            if (hatSlot != null)
            {
                Transform hat = hatSlot.Find("Hat");
                if (worldCat.hat != null)
                {
                    hat.gameObject.SetActive(true);
                    SpriteRenderer sr = hat.GetComponent<SpriteRenderer>();
                    sr.enabled = true;
                    sr.sprite = item.runtimeData.template.icon;
                }
            }
        }


        // Left Weapon Slot
        if(slotName == "L_Weapon")
        {
            Transform leftWeaponSlot = worldCat.transform.Find("L_WeaponSlot");
            if (leftWeaponSlot != null)
            {
                Transform weapon = leftWeaponSlot.Find("Weapon");   
                if (worldCat.weaponL != null)
                {
                    weapon.gameObject.SetActive(true);
                    SpriteRenderer sr = weapon.GetComponent<SpriteRenderer>();
                    sr.enabled = true;
                    sr.sprite = item.runtimeData.template.icon;
                }
            }
        }


        // Right Weapon Slot
        if(slotName == "R_Weapon")
        {
            Transform rightWeaponSlot = worldCat.transform.Find("R_WeaponSlot");
            if (rightWeaponSlot != null)
            {
                Transform weapon = rightWeaponSlot.Find("Weapon");
                if (worldCat.weaponR != null)
                {
                    weapon.gameObject.SetActive(true);
                    SpriteRenderer sr = weapon.GetComponent<SpriteRenderer>();
                    sr.enabled = true;
                    sr.sprite = item.runtimeData.template.icon;
                }
            }
        }

    }
}
