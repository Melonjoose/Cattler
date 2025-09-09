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

    //When the game starts, this script will be used to manage the team of cats

    // Detect which cats are in the team
    public void DetectTeam()
    {
        //check teamCats array for cats.
    }

    // Method to add a cat to the team
    // Add each cat to the team from DetectTeam's list
    public void AddCatToTeam(CatUnit cat)
    {
        if (cat != null)
        {
            Debug.Log("Adding cat to team: " + cat.name);

        }
        else
        {
            Debug.LogWarning("Attempted to add a null cat to the team.");
        }
    }
}
