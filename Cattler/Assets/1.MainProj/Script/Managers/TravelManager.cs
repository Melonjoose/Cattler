using Spine.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic; // only works in the Editor

[System.Serializable]
public class MapLevel
{
[Header("Level Settings")]
    public string mapName;
    public int milestoneDistance;  //7 //17  (+3) to  become //10 //20  etc.
    public Sprite thisLevelFloorSprite;
    public Sprite thisLevelBackgroundSprite;
    public Sprite transitionFloorSprite;
    public Sprite transitionBackgroundSprite;
    public Sprite finalFloorSprite;
    public Sprite finalBackgroundSprite;
    public string milestoneText;
    public string popupMessage;

    public bool transitionTriggered = false;
    public bool textTriggered = false;
}


public class TravelManager : MonoBehaviour
{
    public bool disableTravel = false;
    public bool isTraveling = false;
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

    public List<MapLevel> mapLevels = new List<MapLevel>();
    [SerializeField]private int currentLevelIndex = 0;

    public Animator animator;
    public TextMeshProUGUI text;

    public event Action<bool> OnTravelStateChanged;

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


    private float floorLength = 24.80f; // adjust based on your tile size

    public static TravelManager instance;

    private void Start()
    {
        instance = this;
        text.gameObject.SetActive(false);
        firstFloor = floors[0];
        middleFloor = floors[1];
        lastFloor = floors[2];

        ResetToStart();

        // Explicitly show level 1 popup at boot
        TextPopUp(mapLevels[0].popupMessage);
        UpdateMilestoneText(mapLevels[0].milestoneText);
        mapLevels[0].textTriggered = true;
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

    private void Update()
    {
    
        if (disableTravel) { IsTraveling = false; return; }
        else { IsTraveling = !EnemyDetector.instance.enemyDetected; }

        if (IsTraveling) TeamWalk(); //teamwalk

        if (floors[0].transform.position.x <= -31f) ExtendFloorPlane();  //shift plane to back once hit certain X

        Distance.instance.UpdateDistanceUI(distanceTraveledUIvalue);

        if(disableTravel == false && GameManager.instance.inBattle)
        {
            // Loop through levels
            foreach (var level in mapLevels)
            {
                if (!level.transitionTriggered && distanceTraveledUIvalue > level.milestoneDistance)
                {
                    TransitionToNewLevel(level);
                    level.transitionTriggered = true;
                }

                if (!level.textTriggered && distanceTraveledUIvalue > level.milestoneDistance + 3f)
                {
                    TextPopUp(level.popupMessage);
                    UpdateMilestoneText(level.milestoneText);
                    level.textTriggered = true;
                }
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
        GameObject movedFloor = floors[0];

        // Shift array
        for (int i = 0; i < floors.Length - 1; i++)
            floors[i] = floors[i + 1];

        // Place moved floor
        GameObject newLastFloor = floors[floors.Length - 2];
        movedFloor.transform.position = newLastFloor.transform.position + Vector3.right * floorLength;
        floors[floors.Length - 1] = movedFloor;

        // Apply current level sprites
        MapLevel currentLevel = mapLevels[currentLevelIndex];
        SpriteRenderer floorRenderer = movedFloor.GetComponent<SpriteRenderer>();
        if (floorRenderer != null && currentLevel.finalFloorSprite != null)
            floorRenderer.sprite = currentLevel.finalFloorSprite;

        Transform bgTransform = movedFloor.transform.Find("Background");
        if (bgTransform != null)
        {
            SpriteRenderer bgRenderer = bgTransform.GetComponent<SpriteRenderer>();
            if (bgRenderer != null && currentLevel.finalBackgroundSprite != null)
                bgRenderer.sprite = currentLevel.finalBackgroundSprite;
        }

        // Update references
        firstFloor = floors[0];
        middleFloor = floors[1];
        lastFloor = floors[floors.Length - 1];
    }



    public void TransitionToNewLevel(MapLevel level)
    {
        lastFloor = floors[floors.Length - 1];

        // Transition tile
        SpriteRenderer floorRenderer = lastFloor.GetComponent<SpriteRenderer>();
        if (floorRenderer != null)
            floorRenderer.sprite = level.transitionFloorSprite;

        Transform bgTransform = lastFloor.transform.Find("Background");
        if (bgTransform != null)
        {
            SpriteRenderer bgRenderer = bgTransform.GetComponent<SpriteRenderer>();
            if (bgRenderer != null)
                bgRenderer.sprite = level.transitionBackgroundSprite;
        }

        // Switch current level so recycled tiles use final sprites
        currentLevelIndex = mapLevels.IndexOf(level);
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
        foreach (MapLevel level in mapLevels)
        {
            level.transitionTriggered = false;
            level.textTriggered = false;
        }

        distanceTraveled = 0f;
        distanceTraveledUIvalue = 0f;
        floorGRP.transform.position = new Vector3(0, -4f, 0);

        // Reset floor positions
        for (int i = 0; i < floors.Length; i++)
        {
            float xPos = (i - (floors.Length - 1) / 2f) * floorLength;
            floors[i].transform.position = new Vector3(xPos, -4f, 0);
        }

        currentLevelIndex = 0;
        IntializeLevel(mapLevels[0]);

        // Force milestone text for level 1
        UpdateMilestoneText(mapLevels[0].milestoneText);
        TextPopUp(mapLevels[0].popupMessage);
        mapLevels[0].textTriggered = true;
    }

    public void IntializeLevel(MapLevel level)
    {
        foreach (var floor in floorUI)
        {
            floor.sprite = level.finalFloorSprite;
        }
        foreach (var BG in backgroundUI)
        {
            BG.sprite = level.finalBackgroundSprite;
        }

    }

    void UpdateMilestoneText(string milestoneText)
    {
        Distance.instance.mileStoneText.text = milestoneText;
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
