using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    //When the game starts, this script will be used to manage the team of cats

    // Detect which cats are in the team
    public void DetectTeam()
    {
        // Find all CatUnit components in the scene
        CatUnit[] cats = FindObjectsOfType<CatUnit>();
        
        // Loop through each cat and perform necessary initialization
        foreach (CatUnit cat in cats)
        {
            // Initialize or register the cat in the team
            Debug.Log("Detected cat: " + cat.name);
            // Additional logic to manage the team can be added here
        }
    }

    // Method to add a cat to the team
    // Add each cat to the team from DetectTeam's list
        public void AddCatToTeam(CatUnit cat)
    {
        if (cat != null)
        {
            Debug.Log("Adding cat to team: " + cat.name);
            // Logic to add the cat to the team
            // For example, you could maintain a list of cats in the team
            // teamCats.Add(cat);
        }
        else
        {
            Debug.LogWarning("Attempted to add a null cat to the team.");
        }
    }
}
