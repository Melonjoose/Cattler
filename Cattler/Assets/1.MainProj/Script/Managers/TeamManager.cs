using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    [Header("Team Settings")]
    public int currentTeamSize = 0;
    public int availableTeamSlots = 3; // can expand up to 5
    public int maxTeamSlots = 5;
    public GameObject[] teamCats;      // Holds the *actual spawned cats* in the team // add index to teamcats array

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
        //1. find CatContainers[i] that has containerDetector.cs inside.
        //2. check if containerDetector.cs has isOccupied == false
        //3. if false, assign cat to that containerDetector.cs
        //4. to add it, just set the cat's index to container index. if container1 is empty, set catindex1. etc
        //5. add cat to TeamCats[]
    }


}
