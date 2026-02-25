using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Xml;
using Spine.Unity;
using Spine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int currentTeamSize = 0;
    public int availableTeamSlots = 3; // can expand up to 5
    public int maxTeamSlots = 5;
    public List<ContainerDetector> catContainers = new List<ContainerDetector>(); 
    public List<CatUnit> cats = new List<CatUnit>(); 

    public List<CatUnit> deadCats = new List<CatUnit>(); //store cats that died in battle.

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

            newCatUnit = newCatGO.GetComponent<CatUnit>(); //cat gameobject's catunit component.
            newCatUnit.runtimeData = newlyAddedCat.runtimeData;
            SpriteRenderer catSprite = newCatUnit.GetComponent<SpriteRenderer>();
            catSprite.sprite = newCatUnit.runtimeData.template.icon;

            newlyAddedCat.catGO = newCatUnit.gameObject;

            newCatUnit.catGO = newCatUnit.gameObject;

            newCatUnit.weaponL = newlyAddedCat.weaponL;
            newCatUnit.weaponR = newlyAddedCat.weaponR;
            newCatUnit.hat = newlyAddedCat.hat;

            newCatUnit.inventoryIcon = newlyAddedCat.inventoryIcon;
            //update cat's skin based on data info.
            //get cat skeleton.

            newCatUnit.LinkAnimationBody();
            string catSkinName = newCatUnit.runtimeData.template.skinName; //get skin name from data.
            Skin skin = newCatUnit.skeletonAnimation.Skeleton.Data.FindSkin(catSkinName); //find corresponding skin in skeleton.
            if (skin != null)
            {
                newCatUnit.skeletonAnimation.Skeleton.SetSkin(skin);
                newCatUnit.skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                newCatUnit.skeletonAnimation.AnimationState.Apply(newCatUnit.skeletonAnimation.Skeleton);
            }
            else
            {
                Debug.LogWarning($"Skin {catSkinName} not found!");
            }


            Debug.Log("AddCatToWorld");
            //if newlyAddedCat has item equipped. show item on catGO.
            if (newCatUnit.hat != null)
            {
                EquipItem(newCatUnit.hat, newCatUnit, "Hat");
            }
            if (newCatUnit.weaponL != null)
            {
                Debug.Log("AddCatToWorldWithLeftWeapon");
                EquipItem(newCatUnit.weaponL, newCatUnit, "L_Weapon");
            }
            if (newCatUnit.weaponR != null)
            {
                EquipItem(newCatUnit.weaponR, newCatUnit, "R_Weapon");
            }

            if (!cats.Contains(newCatUnit))
            {
                cats.Add(newCatUnit);
            }

            UpdateItemVisuals(newCatUnit);
            AddCatToTeam(newCatUnit, slot);
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

        currentTeamSize++;
        //int slotIndex = emptyContainer.containerIndex;
        int slotIndex = slot.SlotIndex;
        cat.transform.position = catContainers[slotIndex].transform.position;
        catMovement.MoveToDesignatedLocation(slotIndex);
        catContainers[slot.SlotIndex].occupyingCat = cat;
        catMovement.initialCatIndex = slotIndex; //set the initial index to current slot index


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

        currentTeamSize--;
        cats.Remove(cat);
        GameObject worldCat = cat.catGO;
        CatUnit worldCatUnit = worldCat.GetComponent<CatUnit>();

        if (worldCatUnit == null)
        {
            Debug.LogWarning("Cat GameObject does not contain a CatUnit component.");
            return;
        }

        //remove link in the UI.
        CatIconUI.instance?.UnlinkCatFromIcon(worldCatUnit);

        Destroy(worldCat);
        cat.catGO = null;// Destroy the cat GameObject in the world
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


    void EquipItem(Item item, CatUnit worldCat, string slotName)  //Equip item visuals on world cat. if no item, hide the visuals.
    {

        if (item == null || item.runtimeData?.template?.icon == null)
        {
            Debug.LogWarning("Invalid item or missing icon.");
            return;
        }

        Sprite newIcon = item.runtimeData.template.icon; //change icon visual using template.

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
                else
                {
                    Debug.Log("worldCat has no hat");
                    hat.gameObject.SetActive(false);
                    SpriteRenderer sr = hat.GetComponent<SpriteRenderer>();
                    sr.sprite = null;
                    sr.enabled = false;
                }
            }
        }


        // Left Weapon Slot
        if (slotName == "L_Weapon") //checker for what slot to update.
        {
            Transform leftWeaponSlot = worldCat.transform.Find("L_WeaponSlot");
            if (leftWeaponSlot != null)
            {
                Transform weapon = leftWeaponSlot.Find("Weapon");
                SpriteRenderer sr = weapon.GetComponent<SpriteRenderer>();

                if (worldCat.weaponL != null) // weapon equipped
                {
                    weapon.gameObject.SetActive(true);
                    sr.enabled = true;
                    sr.sprite = worldCat.weaponL.runtimeData.template.icon;
                }
                else // no weapon equipped
                {
                    Debug.Log("worldCat has no left weapon");
                    sr.sprite = null;
                    sr.enabled = false;
                    weapon.gameObject.SetActive(false);
                }
            }
        }



        // Right Weapon Slot
        if (slotName == "R_Weapon")
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
                else
                {
                    Debug.Log("worldCat has no right weapon");
                    weapon.gameObject.SetActive(false);
                    SpriteRenderer sr = weapon.GetComponent<SpriteRenderer>();
                    sr.sprite = null;
                    sr.enabled = false;
                }
            }
        }

    }

    public void UpdateItemVisuals(CatUnit worldCat) //update all cats' item visuals in the world.
    {
        //reference for slots
        Transform hatSlot = worldCat.transform.Find("T_HatSlot");
        Transform leftWeaponSlot = worldCat.transform.Find("L_WeaponSlot");
        Transform rightWeaponSlot = worldCat.transform.Find("R_WeaponSlot");
        SpriteRenderer sr;
        if(worldCat.hat != null)
        {
            sr = hatSlot.Find("Hat").GetComponent<SpriteRenderer>();
            sr.sprite = worldCat.hat.runtimeData.template.icon;
        }
        else
        {
            sr = hatSlot.Find("Hat").GetComponent<SpriteRenderer>();
            sr.sprite = null;
        }
        if(worldCat.weaponL != null)
        {
            sr = leftWeaponSlot.Find("Weapon").GetComponent<SpriteRenderer>();
            sr.sprite = worldCat.weaponL.runtimeData.template.icon;
        }
        else
        {
            sr = leftWeaponSlot.Find("Weapon").GetComponent<SpriteRenderer>();
            sr.sprite = null;
        }
        if(worldCat.weaponR != null)
        {
            sr = rightWeaponSlot.Find("Weapon").GetComponent<SpriteRenderer>();
            sr.sprite = worldCat.weaponR.runtimeData.template.icon;
        }
        else
        {
            sr = rightWeaponSlot.Find("Weapon").GetComponent<SpriteRenderer>();
            sr.sprite = null;
        }
    }

    public void StoreToDeadCatsList(CatUnit cat)
    {
        cats.Remove(cat); //remove from team list. 
        deadCats.Add(cat); //add to dead cats list.

        // delete the UI
        //Destroy Inventory icon. can be found inside of catUnit.
        //Item removedcat = cat.inventoryItem;
        // Inventory.instance.ClearFromTeamList(removedcat);
        //Destroy(removedcat.gameObject);
    }

    public void ClearDeadCatsList() //when battle ends, clear all dead cats by deleting them.
    {
        foreach (CatUnit deadCat in deadCats)
        {
            if (deadCat.catGO != null)
            {
                Destroy(deadCat.catGO); //it's safe to destroy dead cat gameobject now.
            }
            
        }
        deadCats.Clear();
    }

    public void ResetCatPosition()
    {
        foreach (CatUnit cat in cats)
        { 
            cat.catMovement.ResetToInitialPosition(); //this resets world cat positions but not UI.
            CatIconUI.instance.ResetAllIconToInitialIndex();
            cat.Unstun();
        }

    }

    public void HealAllCats()
    {
        foreach (CatUnit cat in cats)
        {
            cat.runtimeData.currentHealth = cat.runtimeData.maxHealth;
        }
    }
}
