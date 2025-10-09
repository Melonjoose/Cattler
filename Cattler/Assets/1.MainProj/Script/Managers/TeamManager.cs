using UnityEngine;
using System;
using Unity.VisualScripting;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int currentTeamSize = 0;
    public int availableTeamSlots = 3; // can expand up to 5
    public int maxTeamSlots = 5;

    public GameObject playerTeamGO;

    public GameObject catTemplatePrefab;

    public Action OnTeamAdd;

    private void Awake()
    {
        instance = this;
        teamCats = new GameObject[availableTeamSlots];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SummonManager.instance.Summon();
        }
    }

    // Detect which cats are in the team (from inventory placeholders)
    public void DetectTeamList()
    {
        // Look at Inventory.TeamList. if cat is in slot 1, AddCatToWorld ().
        // Example: loop through inventory slots, find occupied ones,
        // get their ItemData, and then spawn the correct cat prefab.
    }

    // Add a cat unit to the team

    public void AddCatToWorld(CatRuntimeData runtimeCat) 
    {
        if (currentTeamSize >= availableTeamSlots)
        {
            Debug.Log("No free team slots available!");
            return; 
        }

        //  3. Instantiate the prefab in the scene
        GameObject newCatGO = Instantiate(catTemplatePrefab); //instantiate cat in world
        newCatGO.name = runtimeCat.template.itemName; //rename GO

        currentTeamSize++;

        CatUnit newCatUnit = newCatGO.GetComponent<CatUnit>();
        newCatUnit.runtimeData = runtimeCat; // link runtime data

        runtimeCat.template.name = runtimeCat.template.itemName; //name data

        newCatUnit.AssignCat(runtimeCat); // Initialize cat stats

        AddCatToTeam(newCatUnit.gameObject);
        //Get data from Inventory Team
    }

    public void RemoveCatFromWorld(GameObject Cat)
    {

    }

    public GameObject[] teamCats;  // Array of cats, same length as number of containers

    public void AddCatToTeam(GameObject cat)
    {
        var positionManager = PositionManagerV1.instance;
        if (positionManager == null)
        {
            Debug.LogError("PositionManagerV1 instance not found!");
            return;
        }

        // Make sure teamCats array is initialized with correct size
        if (teamCats == null || teamCats.Length != positionManager.containers.Count)
        {
            teamCats = new GameObject[positionManager.containers.Count];
        }

        foreach (var container in positionManager.containers)
        {
            if (!container.IsOccupied)
            {
                CatMovement catMovement = cat.GetComponent<CatMovement>();
                if (catMovement == null)
                {
                    Debug.LogError("The provided GameObject does not have a CatMovement component.");
                    return;
                }

                int containerIndex = container.containerIndex; // Assuming this starts at 0 or 1 depending on setup
                catMovement.MoveToDesignatedLocation(containerIndex);

                // Move cat to container position
                cat.transform.position = container.transform.position;

                // Optional: Parent under PlayerTeam
                if (playerTeamGO != null)
                    cat.transform.SetParent(playerTeamGO.transform);

                // Mark container as occupied
                CatUnit catUnit = cat.GetComponent<CatUnit>();
                container.AddCatToContainer(catUnit);

                // Assign into team array (adjust index if containerIndex starts at 1)
                int arrayIndex = containerIndex - 1;
                teamCats[arrayIndex] = cat;

                // Trigger event
                OnTeamAdd?.Invoke();

                // Update UI
                CatIconUI.instance.IntializeCatIcon();

                Debug.Log($"Added {cat.name} to container {container.containerIndex} (teamCats index {arrayIndex})");

                return;
            }
        }

        Debug.LogWarning("No empty container found for cat!");
    }

}
