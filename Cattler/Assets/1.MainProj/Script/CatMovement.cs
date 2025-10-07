using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public int catIndex; // Current index in the lineup
    private Transform targetLocation;
    private int lastAssignedIndex = -1;
    [SerializeField] private float moveSpeed = 3f;
    public bool canWalk = true;

    // To be assigned by CatPositionManager
    public GameObject worldPositionGRP; // Parent object containing world position transforms
    public Transform worldPos1;
    public Transform worldPos2;
    public Transform worldPos3;
    public Transform worldPos4;
    public Transform worldPos5;

    private bool isCatWalking = false;

    private void Start()
    {
        worldPositionGRP = GameObject.Find("CatPoints_GRP");
    }
    void Update()
    {
        // Smooth movement
        if (targetLocation != null && canWalk)
        {
            {
                transform.position = Vector3.MoveTowards(transform.position,targetLocation.position,moveSpeed * Time.deltaTime); 
            }

            if (Vector3.Distance(transform.position, targetLocation.position) < 0.05f)
            {
                transform.position = targetLocation.position;
            }
        }

        // Only update target if catIndex changed
        if (catIndex != lastAssignedIndex)
        {
            lastAssignedIndex = catIndex;
            AssignTargetPosition();
        }
    }

    public void AssignWorldPositions()
    {
        if( worldPositionGRP != null)
        {
            worldPos1 = worldPositionGRP.GetComponent<Transform>().Find("Position1");
            worldPos2 = worldPositionGRP.GetComponent<Transform>().Find("Position2");
            worldPos3 = worldPositionGRP.GetComponent<Transform>().Find("Position3");
            worldPos4 = worldPositionGRP.GetComponent<Transform>().Find("Position4");
            worldPos5 = worldPositionGRP.GetComponent<Transform>().Find("Position5");
        }
        else
        {
            Debug.LogError("WorldPositionGRP not assigned in CatPosition!");
        }
    }

    public void AssignCatIndex(int index)
    {
        catIndex = index;
    }

    private void AssignTargetPosition()
    {
        switch (catIndex)
        {
            case 0: MoveToDesignatedLocation(worldPos1); break;
            case 1: MoveToDesignatedLocation(worldPos2); break;
            case 2: MoveToDesignatedLocation(worldPos3); break;
            case 3: MoveToDesignatedLocation(worldPos4); break;
            case 4: MoveToDesignatedLocation(worldPos5); break;
            default:
                Debug.LogError($"CatIndex {catIndex} out of range!");
                break;
        }
    }

    public void MoveToDesignatedLocation(Transform target)
    {
        targetLocation = target;
        isCatWalking = true;
    }

    // Optional: instant teleport
    public void TeleportToLocation(Transform target)
    {
        transform.position = target.position;
        isCatWalking = false;
    }
}
