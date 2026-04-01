using Spine.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements; // only works in the Editor


public class TravelManager : MonoBehaviour
{
    public float distanceTraveled;
    public float distanceTraveledUIvalue;
    public float travelSpeed = 1f;
    public float distanceMultiplier = 1f;
    public GameObject[] floors; // floor1, floor2, floor3
    public GameObject firstFloor;  //first of the sequence
    public GameObject middleFloor;
    public GameObject lastFloor;
    public SpriteRenderer[] backgroundUI;
    public SpriteRenderer[] floorUI;
    public GameObject floorGRP;

    public Animator animator;
    public TextMeshProUGUI text;
    
    public Sprite level1FloorSprite;
    public Sprite level1BackgroundSprite;

    public Sprite level1To2BackgroundSprite; //transitionSprite
    public Sprite level1To2FloorSprite; //transitionSprite
    
    public Sprite level2FloorSprite;
    public Sprite level2BackgroundSprite;

    public float level2Milestone = 10f;

    public event Action<bool> OnTravelStateChanged;

    public bool isTraveling = false;

    private bool isTransitioning = false;

    public bool IsTraveling
    {
        get => isTraveling;
        set
        {
            if (isTraveling != value)
            {
                isTraveling = value;
                OnTravelStateChanged?.Invoke(isTraveling);
            }
        }
    }


    public bool disableTravel = false;

    private float floorLength = 24.80f; // adjust based on your tile size

    public static TravelManager instance;

    private void Start()
    {
        instance = this;
        text.gameObject.SetActive(false);
        firstFloor = floors[0];
        middleFloor = floors[1];
        lastFloor = floors[2];
    }

    private void Awake()
    {
        // Initialize floor positions
        for (int i = 0; i < floors.Length; i++)
        {
            float xPos = (i - (floors.Length - 1) / 2f) * floorLength;
            floors[i].transform.position = new Vector3(xPos, 0f, 0);
            //first floor is at (-24.8,-4, 0), second at (0, 0, 0), third at (24.8, -4, 0)
        }

    }

    public bool completedTransitionPlayed = false; // declare at class level
    public bool level2Triggered = false; // class-level field
    public bool level2TextTriggered = false; // class-level field

    private void Update()
    {
        if (disableTravel) { IsTraveling = false; return; }
        else { IsTraveling = !EnemyDetector.instance.enemyDetected; }

        if (IsTraveling) TeamWalk();

        if (floors[0].transform.position.x <= -31f) ExtendFloorPlane();

        Distance.instance.UpdateDistanceUI(distanceTraveledUIvalue);



        if (distanceTraveledUIvalue > level2Milestone  && !isTransitioning && !level2Triggered) //if its not transitiong (false) play it once. then inside transiton to newlevel it will trigger intrantioning = true. causing this to play once. but when completed it trigger intransition to become false which plays this again due to no safeguarding the distance pasttt
        {
            TransitionToNewLevel(2);
            level2Triggered = true;
        }
        if (distanceTraveledUIvalue > (level2Milestone+3f) && !level2TextTriggered) //if its not transitiong (false) play it once. then inside transiton to newlevel it will trigger intrantioning = true. causing this to play once. but when completed it trigger intransition to become false which plays this again due to no safeguarding the distance pasttt
        {
            TextPopUp("The Ruined City");
            level2TextTriggered=true;
        }
        // Check if transition floor has scrolled into view

        if (isTransitioning) //current scenario that doesn't work.. floor 3. is in firstfloor position. which is also the transitiontile. middleFloor is Floor 1, Lvl2tile. based on below code, it will check floor1SR if it is a lvl2floor. it is, hence it will upgrade the transitiontile to lvl2tile. BUT it doesnt.
        {
            // Always check middleFloor
            SpriteRenderer middleFloorSR = middleFloor.GetComponent<SpriteRenderer>();
            if (middleFloorSR.sprite == level1To2FloorSprite || middleFloorSR.sprite == level2FloorSprite)
            {
                UpgradeToLevel2(lastFloor);
            }

            bool allLevel2 = true;
            foreach (GameObject floor in floors)
            {
                SpriteRenderer sr = floor.GetComponent<SpriteRenderer>();
                if (sr.sprite != level2FloorSprite)
                {
                    allLevel2 = false;
                    break;
                }
            }

            if (allLevel2 && !completedTransitionPlayed)
            {
                CompleteTransitionToLevel2();
                completedTransitionPlayed = true; // now it persists
            }
        }
    }


    public void TeamWalk()
    {
        
        distanceTraveled += (travelSpeed*distanceMultiplier) * Time.deltaTime;
        floorGRP.transform.position = new Vector3(-distanceTraveled, -4f, 0); // floor movement
        
        distanceTraveledUIvalue += travelSpeed * Time.deltaTime;
    }

    public void ExtendFloorPlane()
    {
        firstFloor = floors[0];
        lastFloor = floors[floors.Length - 1];

        // Move first floor to the end
        firstFloor.transform.position = lastFloor.transform.position + Vector3.right * floorLength;

        // Shift the list
        for (int i = 0; i < floors.Length - 1; i++)
        {
            floors[i] = floors[i + 1];
        }
        floors[floors.Length - 1] = firstFloor;

        // Update references
        firstFloor = floors[0];
        middleFloor = floors[1];
        lastFloor = floors[2];

    }

    private void UpgradeToLevel2(GameObject floor)
    {
        SpriteRenderer floorRenderer = floor.GetComponent<SpriteRenderer>();
        if (floorRenderer != null)
        {
            floorRenderer.sprite = level2FloorSprite;
        }

        Transform bgTransform = floor.transform.Find("Background");
        if (bgTransform != null)
        {
            SpriteRenderer bgRenderer = bgTransform.GetComponent<SpriteRenderer>();
            if (bgRenderer != null)
            {
                bgRenderer.sprite = level2BackgroundSprite;
            }
        }
    }

    public void TransitionToNewLevel(int level) //the moment it hit milestone, play this function
    {
        if (level == 2 && !isTransitioning)
        {
            lastFloor = floors[floors.Length - 1]; //mark last floor

            // Floor sprite
            SpriteRenderer floorRenderer = lastFloor.GetComponent<SpriteRenderer>(); //find floorSR
            if (floorRenderer != null)
            {
                floorRenderer.sprite = level1To2FloorSprite;  //Change the last floor to new tile.
            }

            // Background sprite (child of lastFloor)
            Transform bgTransform = lastFloor.transform.Find("Background"); //find BackgroundSR
            if (bgTransform != null)
            {
                SpriteRenderer bgRenderer = bgTransform.GetComponent<SpriteRenderer>();
                if (bgRenderer != null)
                {
                    bgRenderer.sprite = level1To2BackgroundSprite; //change backgroundSR
                }
            }

            isTransitioning = true;  //transitioning is still in progress.

        }
    }
    private void CompleteTransitionToLevel2()
    {
        //animator.SetTrigger("Play");
        isTransitioning = false;
        //Debug.Log("Entering Lvl2");
    }

    private void TextPopUp(String textmessage)
    {

        text.gameObject.SetActive(true);
        text.text = textmessage;
        animator.SetTrigger("Play");
        //text animation sequence
    }

    public void ResetToStart()
    {
        //all checks are reset to default
        completedTransitionPlayed = false;
        level2Triggered = false;
        level2TextTriggered = false;

        distanceTraveled = 0f;
        distanceTraveledUIvalue = 0f;
        floorGRP.transform.position = new Vector3(0, -4f, 0);

        // Reset floor positions
        for (int i = 0; i < floors.Length; i++)
        {
            float xPos = (i - (floors.Length - 1) / 2f) * floorLength;
            floors[i].transform.position = new Vector3(xPos, -4f, 0);
            //first floor is at (-23.09,-3.64, 0), second at (0, -3.64, 0), third at (23.09, -3.64, 0)
        }

        EnteringLevel1();
    }
    public void EnteringLevel1()
    {
        foreach (var floor in floorUI)
        {
            floor.sprite = level1FloorSprite;
        }
        foreach (var BG in backgroundUI)
        {
            BG.sprite = level1BackgroundSprite;
        }

        text.gameObject.SetActive(true);
        text.text = "The Safe Heaven";
        animator.SetTrigger("Play");
    }

    public void DisableTravel()
    {
        disableTravel = true;
        Debug.Log("Travel Disabled");
    }
    public void EnableTravel()
    {
        disableTravel = false;
    }
}
