using UnityEngine;

public class EnemyTriggerTrack : MonoBehaviour
{
    private GameObject enemy;
    public float triggerRadius = 5f;      // Radius around the cat
    public float moveSpeed = 3f;
    public Vector3 offsetFromCat = new Vector3(0, 0, 1f); // Position in front of the cat

    private GameObject chosenCat;
    private EnemyUnit enemyUnit; // Assuming you have an EnemyData class that holds enemy properties

    void Start()
    {
        enemy = transform.parent.gameObject; // Automatically assign the enemy as parent
    }

    void FixedUpdate()
    {
        FindChosenCat();
        TrackEnemy();
    }

    void TrackEnemy()
    {
        if (chosenCat != null)
        {
            // Move toward the enemy
            transform.position = Vector3.MoveTowards(transform.position, chosenCat.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Return to position in front of the cat
            Vector3 targetPosition = enemy.transform.position +
                                     enemy.transform.forward * offsetFromCat.z +
                                     enemy.transform.right * offsetFromCat.x +
                                     enemy.transform.up * offsetFromCat.y;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void FindChosenCat()
    {
        GameObject[] allCats = GameObject.FindGameObjectsWithTag("Cat");

        float closestDistance = Mathf.Infinity;
        GameObject closestCat = null;

        foreach (GameObject cat in allCats)
        {
            float distance = Vector3.Distance(enemy.transform.position, cat.transform.position);
            if (distance < closestDistance && distance <= triggerRadius)
            {
                closestDistance = distance;
                closestCat = cat;
            }
        }

        chosenCat = closestCat; // Will be null if no enemies are in range
    }
}
