using System;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public int catIndex; // Current index in the lineup
    public bool inPosition = false;
    public Rigidbody2D rb;
    private CatUnit catUnit => GetComponent<CatUnit>();
    private Transform targetLocation;
    private int lastAssignedIndex = -1;
    [SerializeField] private float moveSpeed = 3f;
    public bool canWalk = true;

    // To be assigned by CatPositionManager
    public Transform PlayerTeam;
    public List<Transform> worldPositions = new List<Transform>();
    public event Action onMove;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AssignWorldPositionsAndIndex();
        lastAssignedIndex = catIndex;
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
        PlayerTeam = GameObject.Find("PlayerTeam")?.transform;
        if (PlayerTeam == null)
        {
            Debug.LogError("PlayerTeam not assigned in CatPosition!");
            return;
        }

        worldPositions.Clear(); // Clear any previous data

        // Loop through expected child names
        for (int i = 0; i < 5; i++)
        {
            Transform pos = PlayerTeam.Find($"Cat_Container{i}");
            if (pos != null)
            {
                worldPositions.Add(pos); //
                Debug.Log($"Assigned {pos.name} as index {i}"); 
            }
            else
            {
                Debug.LogWarning($"Position{i} not found under {PlayerTeam.name}");
            }
        }
    }
    public void MoveToDesignatedLocation(int targetindex)
    {
        inPosition = false;
        lastAssignedIndex = catIndex; // make lastAssignedIndex same as catIndex so it can update
        AssignCatIndex(targetindex);
        onMove?.Invoke();
    }

    public void AssignCatIndex(int index) //where the cat is in the lineup
    {
        Debug.Log($"Assigning {gameObject.name} to index {index}");
        catIndex = index;
    }



    void Walk(int targetindex)
    {
        targetLocation = worldPositions[targetindex]; // 0 - 4

        if (targetLocation != null && canWalk)
        {
            {
                Vector2 currentPos = rb.position;
                Vector2 targetPos = targetLocation.position;

                // Move only X
                float newX = Mathf.MoveTowards(currentPos.x, targetPos.x, moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(new Vector2(newX, currentPos.y));
            }

            if (Vector3.Distance(transform.position, targetLocation.position) < 0.05f)
            {
                transform.position = targetLocation.position;
            }
        }
    }

}
