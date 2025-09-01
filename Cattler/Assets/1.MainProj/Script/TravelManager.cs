using UnityEngine;

public class TravelManager : MonoBehaviour
{
    public float distanceTraveled;
    public float travelSpeed = 1f;
    public GameObject[] floors; // floor1, floor2, floor3
    public GameObject floorGRP;
    public bool isTraveling = false;

    private float floorLength = 30f; // adjust based on your tile size

    void Update()
    {
        if (isTraveling)
        {
            TeamWalk();
        }

        // Check the first floor in the list
        if (floors[0].transform.position.x <= -31f)
        {
            ExtendFloorPlane();
            
        }

        Distance.instance.UpdateDistanceUI(distanceTraveled);
    }

    public void TeamWalk()
    {
        distanceTraveled += travelSpeed * Time.deltaTime;
        floorGRP.transform.position = new Vector3(-distanceTraveled, -3.64f, 0);
        
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
