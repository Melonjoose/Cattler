using UnityEngine;
using System;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int currentTeamSize = 0;
    public int availableTeamSlots = 3; // can expand up to 5
    public int maxTeamSlots = 5;
    public GameObject[] teamCats;      // Holds the *actual spawned cats* in the team // add index to teamcats array
    public GameObject playerTeamGO;

    public GameObject catTemplatePrefab;

    public Action onTeamAdd;

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

        CatUnit newCatUnit = newCatGO.GetComponent<CatUnit>();
        newCatUnit.runtimeData = runtimeCat; // link runtime data

        runtimeCat.template.name = runtimeCat.template.itemName; //name data

        newCatUnit.AssignCat(runtimeCat); // Initialize cat stats

        AddCatToTeam(newCatUnit.gameObject);
        //Get data from Inventory Team
        currentTeamSize++;
    }

    public void RemoveCatFromWorld(GameObject Cat)
    {

    }

    public void AddCatToTeam(GameObject cat)
    {
        var positionManager = PositionManagerV1.instance;
        if (positionManager == null)
        {
            Debug.LogError("PositionManagerV1 instance not found!");
            return;
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

                // Assign container index
                //assigning catIndex to be container's index if container is 1, catindex is 1.
                //catMovement.catIndex = container.containerIndex; //containerIndex are 1 ,2,3,4,5 not sure if I have to plus 1
                catMovement.MoveToDesignatedLocation(container.containerIndex);

                // Move cat to container position (for initial placement)
                cat.transform.position = container.transform.position;

                // Optional: Parent under PlayerTeam instead of container
                if (playerTeamGO != null)
                    cat.transform.SetParent(playerTeamGO.transform);

                // Mark container as occupied
                CatUnit catUnit = cat.GetComponent<CatUnit>();
                container.AddCatToContainer(catUnit);

                onTeamAdd?.Invoke();
                Debug.Log($"Added {cat.name} to container {container.containerIndex}");

                //  Exit after assigning to the first available container
                return;
            }
        }

        // If no container was found
        Debug.LogWarning("No empty container found for cat!");
    }



}
