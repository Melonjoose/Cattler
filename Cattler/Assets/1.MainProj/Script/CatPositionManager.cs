using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CatPositionManager : MonoBehaviour
{
    public static CatPositionManager instance;

    [Header("WorldPosition")]
    public List<GameObject> worldPositions = new List<GameObject>(); // world positions for cats to walk to.

    [Header("CurrentCatPositions")]
    public List<GameObject> catPositions = new List<GameObject>(); // Where the cats are supposed to be.

    public GameObject playerTeam; // Reference to the player team GameObject

    void Start()
    {
        instance = this;
        playerTeam = this.gameObject; // Assuming this script is attached to the player team GameObject
        AssignWorldPositions();
        UpdateCatPositions();
    }


    public void AssignWorldPositions() //ONE TIME USE at start.
    {
        worldPositions.Clear();

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
                worldPositions.Add(child.gameObject);
            }
        }
    }

    public void UpdateCatPositions() // Call this whenever cats are added/removed
    {
        catPositions.Clear();

        Transform playerTeamGRP = playerTeam.transform.Find("PlayerTeam");
        foreach (Transform cat in playerTeamGRP)
        {
            if (cat.CompareTag("Cat")) // only add if tagged correctly
            {
                catPositions.Add(cat.gameObject);
            }
        }
    }

    public void SyncCatsWithIcons(List<DraggableIcon> icons)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            CatPosition cat = icons[i].GetAssignedCat();
            if (cat == null) continue;

            // Move to correct position
            if (i < worldPositions.Count)
                cat.MoveToDesignatedLocation(worldPositions[i].transform);
        }
    }

}
