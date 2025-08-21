using UnityEngine;

public class CatPosition : MonoBehaviour
{
    public int CatPositionIndex;
    public bool isCatWalking = false;
    public bool isCatAtTargetLocation = false;

    [SerializeField] private float moveSpeed = 3f;
    private Transform targetLocation;

    void Update()
    {
        // Move smoothly to target if one is set
        if (isCatWalking && targetLocation != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetLocation.position,
                moveSpeed * Time.deltaTime
            );

            // Check if reached
            if (Vector3.Distance(transform.position, targetLocation.position) < 0.05f)
            {
                transform.position = targetLocation.position;
                isCatWalking = false;
                isCatAtTargetLocation = true;
            }
        }
    }

    public void SetCatPositionIndex(int index)
    {
        CatPositionIndex = index;
    }

    public void MoveToDesignatedLocation(Transform target)
    {
        targetLocation = target;
        isCatWalking = true;
        isCatAtTargetLocation = false;
    }

    public void TeleportToLocation(Transform target)
    {
        transform.position = target.position;
        isCatWalking = false;
        isCatAtTargetLocation = true;
    }
}