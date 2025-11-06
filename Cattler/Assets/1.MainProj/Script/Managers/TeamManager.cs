using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

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

    public void AddCatToWorld(CatUnit newlyAddedCat, int slot) 
    {
        if (currentTeamSize >= availableTeamSlots)
        {
            Debug.Log("No free team slots available!");
            return; 
        }

        //  3. Instantiate the prefab in the scene
        GameObject newCatGO = Instantiate(catTemplatePrefab); //instantiate cat in world
        newCatGO.name = newlyAddedCat.runtimeData.template.itemName; //rename GO

        currentTeamSize++;

        CatUnit newCatUnit = newCatGO.GetComponent<CatUnit>();
        newCatUnit.runtimeData = newlyAddedCat.runtimeData; // link runtime data. Get from TeamManager.
        SpriteRenderer catSprite = newCatUnit.GetComponent<SpriteRenderer>();
        catSprite.sprite = newCatUnit.runtimeData.template.icon;

        AddCatToTeam(newCatUnit , slot);

        cats.Add(newCatUnit );

        newlyAddedCat.catGO = newCatUnit.gameObject;

    }

    public void AddCatToTeam(CatUnit cat , int slot)  // Team is not empty & ONLY to be added into the world when battle begin 
    {
        var emptyContainer = FindEmptyContainer();
        if (emptyContainer == null)
        {
            Debug.Log("no empty containers");
        }

        CatMovement catMovement = cat.GetComponent<CatMovement>();
        if (catMovement == null)
        {
            Debug.LogError("The provided GameObject does not have a CatMovement component.");
            return;
        }

        //int slotIndex = emptyContainer.containerIndex;
        int slotIndex = slot;
        cat.transform.position = catContainers[slotIndex].transform.position;
        catMovement.MoveToDesignatedLocation(slotIndex);
        emptyContainer.occupyingCat = cat;
        

        // Move cat to container position

        // Optional: Parent under PlayerTeam
        if (playerTeamGO != null)
            cat.transform.SetParent(playerTeamGO.transform);

        CatIconUI.instance.LinkCatToIcon(slotIndex , cat);

        return;

    }

    public void RemoveCatFromWorld(CatUnit cat)
    {
        GameObject worldCat = cat.catGO;
        CatUnit WorldCatUnit = worldCat.GetComponent<CatUnit>();
        CatIconUI.instance.UnlinkCatFromIcon(WorldCatUnit);
        cat.catGO = null;
        Destroy(WorldCatUnit.gameObject);
        cats.Remove(cat); //remove from the list

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
}
