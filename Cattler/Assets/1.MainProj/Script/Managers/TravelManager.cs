using UnityEngine;

public class TravelManager : MonoBehaviour
{
    public float distanceTraveled;
    public float distanceTraveledUIvalue;
    public float travelSpeed = 1f;
    public float distanceMultiplier = 1f;
    public GameObject[] floors; // floor1, floor2, floor3
    public GameObject floorGRP;

    public bool isTraveling = false;

    public bool disableTravel = false;

    private float floorLength = 24.80f; // adjust based on your tile size

    public static TravelManager instance;

    private void Start()
    {
        instance = this;
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


    void Update()
    {
        if (disableTravel == true)
        {
            return;
        }
        else
        {
            if (EnemyDetector.instance.enemyDetected == false)//&& any cat is not attacking)
            {
                isTraveling = true;
            }
            else
            {
                isTraveling = false;
            }
        }

        if (isTraveling)
        {
            TeamWalk();
        }

        // Check the first floor in the list
        if (floors[0].transform.position.x <= -31f)
        {
            ExtendFloorPlane();
            
        }

        Distance.instance.UpdateDistanceUI(distanceTraveledUIvalue);
    }

    public void TeamWalk()
    {
        distanceTraveled += (travelSpeed*distanceMultiplier) * Time.deltaTime;
        floorGRP.transform.position = new Vector3(-distanceTraveled, -4f, 0); // floor movement
        
        distanceTraveledUIvalue += travelSpeed * Time.deltaTime;
    }

    public void ExtendFloorPlane()
    {
        // take the first floor (the leftmost one)
        GameObject firstFloor = floors[0];

        // find the last floor
        GameObject lastFloor = floors[floors.Length - 1];

        // move first floor to the end
        firstFloor.transform.position = lastFloor.transform.position + Vector3.right * floorLength;

        // shift the list so the new order is maintained
        for (int i = 0; i < floors.Length - 1; i++)
        {
            floors[i] = floors[i + 1];
        }
        floors[floors.Length - 1] = firstFloor;
    }

    public void TransitionToNewLevel()
    {
        // Logic to transition to a new level, e.g., load new scene or change environment
    }

    public void ResetToStart()
    {
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
