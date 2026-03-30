using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public int catIndex; // Current index in the lineup
    public int initialCatIndex; // Initial index to reset to
    public bool inPositionNewPosition = false;
    public Rigidbody2D rb;
    private CatUnit catUnit => GetComponent<CatUnit>();
    private Transform targetLocation;
    private int lastAssignedIndex = -1;
    [SerializeField] private float moveSpeed = 3f;
    public bool canWalk = true;

    public bool isWalking = false;

    // To be assigned by CatPositionManager
    public Transform PlayerTeam;
    public List<Transform> worldPositions = new List<Transform>();
    public event Action onMove;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AssignWorldPositionsAndIndex();
        lastAssignedIndex = catIndex;
        initialCatIndex = catIndex;
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
                //Debug.Log($"Assigned {pos.name} as index {i}"); 
            }
            else
            {
                Debug.LogWarning($"Position{i} not found under {PlayerTeam.name}");
            }
        }
    }
    public void MoveToDesignatedLocation(int targetindex)
    {
        
        CatIconUI.instance.MoveIcon(catUnit,targetindex);
        inPositionNewPosition = false;
        AssignCatIndex(targetindex);
        lastAssignedIndex = catIndex; // make lastAssignedIndex same as catIndex so it can update


        onMove?.Invoke();
    }

    public void AssignCatIndex(int index) //where the cat is in the lineup
    {
        Debug.Log($"Assigning {gameObject.name} to index {index}");
        catIndex = index;
        InPositionFirstTime();
    }

    void Walk(int targetIndex)
    {
        targetLocation = worldPositions[targetIndex];

        if (targetLocation != null && canWalk)
        {
            Vector2 currentPos = rb.position;
            Vector2 targetPos = targetLocation.position;

            float newX = Mathf.MoveTowards(currentPos.x, targetPos.x, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(new Vector2(newX, currentPos.y));
        }

    }


    public void ResetToInitialPosition()
    {
        catIndex = initialCatIndex; // they slowly walk to their original location. but now I want them to immediately position themself.
        catUnit.gameObject.transform.position = worldPositions[catIndex].position;
    }

    [SerializeField]private bool inPositionFirstTimeCompleted = false;
    public void InPositionFirstTime()
    {
        Debug.Log($"InPositionFirstTime called for {gameObject.name}. Tutorial stage: {Tutorial.instance.tutorialStage}, inTutorial: {Tutorial.instance.inTutorial}, inPositionFirstTimeCompleted: {inPositionFirstTimeCompleted}");
        if (Tutorial.instance.tutorialStage == 10 && Tutorial.instance.inTutorial && !inPositionFirstTimeCompleted) //needs to be in tutorial and right stage.
        {
            inPositionFirstTimeCompleted = true;
            Tutorial.instance.TriggerTUT11();
            Debug.Log($"InPositionFirstTime triggered TUT11 for {gameObject.name}");
        }
    }
}
