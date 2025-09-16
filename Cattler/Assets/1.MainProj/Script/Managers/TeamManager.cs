using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int availableTeamSlots = 3; // can expand up to 5
    public GameObject[] teamCats;      // Holds the *actual spawned cats* in the team

    public GameObject catTemplatePrefab;

    private void Awake()
    {
        instance = this;
        teamCats = new GameObject[availableTeamSlots];
    }

    private void Update()
    {
        // if cats are added into the cat team, add Cat to world. AddCatToWorld(CatRuntimeData)
        //if cats are removed from the cat team, remove Cat from world. RemoveCatFromWorld(CatUnit)
    }

    // Detect which cats are in the team (from inventory placeholders)
    public void DetectTeam()
    {
        // This depends on how you structured inventory.
        // Example: loop through inventory slots, find occupied ones,
        // get their ItemData, and then spawn the correct cat prefab.
    }

    // Add a cat unit to the team

    public void AddCatToWorld(CatRuntimeData runtimeCat)
    {
        //  3. Instantiate the prefab in the scene
        GameObject newCatGO = Instantiate(catTemplatePrefab);
        //Get data from Inventory Team
    }

    public void RemoveCatFromWorld(GameObject Cat)
    {

    }

    public void AddCatToTeam(CatUnit cat)
    {
        for (int i = 0; i < availableTeamSlots; i++)
        {
            if (teamCats[i] == null) // Empty slot
            {
                // Parent cat to CatPositionManager’s container
                cat.transform.SetParent(CatPositionManager.instance.catContainers[i].transform);
                cat.transform.localPosition = Vector3.zero; // snap into container

                teamCats[i] = cat.gameObject;

                Debug.Log($"Added {cat.runtimeData.unitName} to the team at slot {i}!");
                return;
            }
        }

        Debug.LogWarning("No free team slots available!");
    }


}
