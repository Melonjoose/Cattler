using System;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public int catIndex; // Current index in the lineup
    public bool inPosition = false;
    private CatUnit catUnit => GetComponent<CatUnit>();
    private Transform targetLocation;
    private int lastAssignedIndex = -1;
    [SerializeField] private float moveSpeed = 3f;
    public bool canWalk = true;

    // To be assigned by CatPositionManager
    public Transform worldPositionGRP;
    public List<Transform> worldPositions = new List<Transform>();
    public Action<int> onMove;

    private void Start()
    {
        AssignWorldPositionsAndIndex();
    }
    void Update()
    {
        Walk(catIndex); // walks to catIndex position

        // Only update target if catIndex changed
        if (catIndex != lastAssignedIndex)
        {
            lastAssignedIndex = catIndex;
        }
    
        moveSpeed = catUnit.runtimeData.movementSpeed;
    }
    
    public void AssignWorldPositionsAndIndex() //assign 1-5 positions to worldPositions list
    {
        worldPositionGRP = GameObject.Find("CatPoints_GRP")?.transform;
        if (worldPositionGRP == null)
        {
            Debug.LogError("WorldPositionGRP not assigned in CatPosition!");
            return;
        }

        worldPositions.Clear(); // Clear any previous data

        // Loop through expected child names
        for (int i = 1; i <= 5; i++)
        {
            Transform pos = worldPositionGRP.Find($"Position{i}");
            if (pos != null)
            {
                worldPositions.Add(pos);
                Debug.Log($"Assigned {pos.name} as index {i}");
            }
            else
            {
                Debug.LogWarning($"Position{i} not found under {worldPositionGRP.name}");
            }
        }
    }

    public void AssignCatIndex(int index) //where the cat is in the lineup
    {
        Debug.Log($"Assigning {gameObject.name} to index {index}");
        catIndex = index;
    }

    public void MoveToDesignatedLocation(int targetindex) 
    {
        lastAssignedIndex = catIndex; // make lastAssignedIndex same as catIndex so it can update
        catIndex = targetindex; // change catIndex to targetindex so it can walk to that position
        onMove?.Invoke(targetindex);
    }

    void Walk(int targetindex)
    {
        targetLocation = worldPositions[targetindex-1]; // -1 because list is 0-based //go to

        if (targetLocation != null && canWalk)
        {
            {
                transform.position = Vector3.MoveTowards(transform.position, targetLocation.position, moveSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, targetLocation.position) < 0.05f)
            {
                transform.position = targetLocation.position;
            }
        }
    }

}
