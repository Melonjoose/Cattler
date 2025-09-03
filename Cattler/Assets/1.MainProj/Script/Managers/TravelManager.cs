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

    private float floorLength = 23.09f; // adjust based on your tile size

    void Update()
    {
        if (EnemyDetector.instance.enemyDetected == false )//&& any cat is not attacking)
        {
            isTraveling = true;
        }
        else
        { 
            isTraveling = false; 
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
        floorGRP.transform.position = new Vector3(-distanceTraveled, -3.64f, 0); // floor movement
        
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

}
