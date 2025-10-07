using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int currentTeamSize = 0;
    public int availableTeamSlots = 3; // can expand up to 5
    public int maxTeamSlots = 5;
    public GameObject[] teamCats;      // Holds the *actual spawned cats* in the team

    public GameObject catTemplatePrefab;

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
    public void DetectTeam()
    {
        // This depends on how you structured inventory.
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
        GameObject newCatGO = Instantiate(catTemplatePrefab);
        newCatGO.name = runtimeCat.template.itemName; //rename GO

        CatUnit newCatUnit = newCatGO.GetComponent<CatUnit>();
        newCatUnit.runtimeData = runtimeCat; // link runtime data

        runtimeCat.template.name = runtimeCat.template.itemName; //name data

        newCatUnit.AssignCat(runtimeCat); // Initialize cat stats

        AddCatToTeam(newCatUnit);
        //Get data from Inventory Team
        currentTeamSize++;
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

                cat.GetComponent<CatMovement>().AssignCatIndex(i + 1); // +1 because index is 1-based

                Debug.Log($"Added {cat.runtimeData.template.itemName} to the team at slot {i}!");
                return;
            }
        }

        Debug.LogWarning("No free team slots available!");
    }


}
