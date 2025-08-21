using System.Collections.Generic;
using UnityEngine;

public class CatPositionManager : MonoBehaviour
{
    [Header("Position Assignments")]
    public List<GameObject> positions = new List<GameObject>();

    [Header("Cat Designated Positions")]
    public List<GameObject> catDesignatedPosition = new List<GameObject>();

    public GameObject playerTeam; // Reference to the player team GameObject

    void Start()
    {
        playerTeam = this.gameObject; // Assuming this script is attached to the player team GameObject
        AutoAssignPositions();
        AssignCatTeam();
        MoveCatsToPositionAssigned();
    }

    public void AssignCatTeam()
    {
        catDesignatedPosition.Clear();

        int catCounter = 0;

        foreach (Transform child in playerTeam.transform)
        {
            if (child.CompareTag("Cat"))
            {
                CatPosition cat = child.GetComponent<CatPosition>();
                if (cat != null)
                {
                    if (catCounter < positions.Count)
                    {
                        cat.SetCatPositionIndex(catCounter); // assign sequential index
                        catDesignatedPosition.Add(child.gameObject);

                        Debug.Log($"{child.name} assigned to index {catCounter}");
                    }
                    else
                    {
                        Debug.LogWarning("More cats than available positions!");
                    }

                    catCounter++;
                }
            }
        }
    }

    public void AutoAssignPositions()
    {
        positions.Clear();

        Transform catpointsGroup = playerTeam.transform.Find("CatPoints_GRP");
        if (catpointsGroup == null)
        {
            Debug.LogError("Catpoints_GRP not found under " + playerTeam.name);
            return;
        }

        foreach (Transform child in catpointsGroup)
        {
            if (child.name.StartsWith("Position"))
            {
                positions.Add(child.gameObject);
            }
        }
    }

    void MoveCatsToPositionAssigned()
    {
        foreach (GameObject catObj in catDesignatedPosition)
        {
            CatPosition cat = catObj.GetComponent<CatPosition>();
            if (cat != null)
            {
                int index = cat.CatPositionIndex;

                if (index >= 0 && index < positions.Count)
                {
                    Transform pos = positions[index].transform;

                    // Option A: smooth walk
                    cat.MoveToDesignatedLocation(pos);

                    // Option B: instant snap
                    // cat.TeleportToLocation(pos);
                }
                else
                {
                    Debug.LogWarning("Invalid position index for " + catObj.name);
                }
            }
        }
    }

    //Read 
}
