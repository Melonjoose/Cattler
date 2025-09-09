using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;
    public int availableTeamSlots = 3; // Maximum number of cats allowed in the team. can be upgraded later. max 5.
    public GameObject[] teamCats; // Array to hold the cats in the team

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
           
        }
    }

    //When the game starts, this script will be used to manage the team of cats

    // Detect which cats are in the team
    public void DetectTeam()
    {
        //check catSlotPlaceHolders in Inventory has cats.
        //add each cat to teamCats array
    }

    // Method to add a cat to the team
    // Add each cat to the team from DetectTeam's list
    public void AddCatToTeam(CatUnit cat)
    {
        cat.transform.SetParent(CatPositionManager.instance.catContainer1.transform); // Set the cat's parent to the first container for now
        cat.transform.localPosition = Vector3.zero; // Reset local position

        for (int i = 0; i < teamCats.Length; i++)
        {
            if (teamCats[i] == null) // Find the first empty slot
            {
                //for now we will just add cat directly from summoning.

                teamCats[i] = cat.gameObject; // Add the cat to the team //BUTBUT BUT the problem is that the item in inventory is a placeholder, not the actual cat. so I need to find a way to get the actual cat from the placeholder.
                Debug.Log($"Added {cat.runtimeData.unitName} to the team!");
                return;
            }
        }
    }
}
