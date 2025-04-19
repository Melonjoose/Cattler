using UnityEngine;

public class TriggerTrack : MonoBehaviour
{
    private GameObject cat;
    public float triggerRadius = 5f;      // Radius around the cat
    public float moveSpeed = 3f;
    public Vector3 offsetFromCat = new Vector3(0, 0, 1f); // Position in front of the cat

    private GameObject nearestEnemy;

    void Start()
    {
        cat = transform.parent.gameObject; // Automatically assign the cat as parent
    }

    void FixedUpdate()
    {
        FindNearestEnemy();
        TrackEnemy();
    }

    void TrackEnemy()
    {
        if (nearestEnemy != null)
        {
            // Move toward the enemy
            transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Return to position in front of the cat
            Vector3 targetPosition = cat.transform.position +
                                     cat.transform.forward * offsetFromCat.z +
                                     cat.transform.right * offsetFromCat.x +
                                     cat.transform.up * offsetFromCat.y;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector3.Distance(cat.transform.position, enemy.transform.position); // Use cat's position
            if (distance < closestDistance && distance <= triggerRadius)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        nearestEnemy = closestEnemy; // Will be null if no enemies are in range
    }
}
