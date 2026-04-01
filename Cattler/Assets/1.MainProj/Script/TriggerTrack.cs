using UnityEngine;

public class TriggerTrack : MonoBehaviour
{
    public bool canAttack = true;
    private GameObject cat;
    private CatUnit catUnit;
    public float triggerRadius = 5f;      // Radius around the cat
    public bool inRange = false;         // Whether the enemy is in range for attack
    public float moveSpeed = 3f;
    public Vector3 offsetFromCat = new Vector3(0, 0, 1f); // Position in front of the cat

    private GameObject nearestEnemy;


    void Start()
    {
        cat = transform.parent.gameObject; // Automatically assign the cat as parent
        catUnit = cat.GetComponent<CatUnit>();
    }

    void FixedUpdate()
    {

        if (triggerRadius != catUnit.runtimeData.attackRange)
        {
            triggerRadius = catUnit.runtimeData.attackRange;
        }
        FindNearestEnemy();
        TrackEnemy();
        RevealTracker();
    }

    void TrackEnemy()
    {
        if (nearestEnemy != null && canAttack)
        {
            inRange = true;
            // Move toward the enemy
            transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == nearestEnemy.transform.position)
            { 
                transform.position = nearestEnemy.transform.position; // to lock on so it don't jitter
            }
        }
        else
        {

            inRange = false;
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
                EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
                if (enemyUnit != null && !enemyUnit.isDead) // Check if enemy is alive
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        nearestEnemy = closestEnemy; // Will be null if no enemies are in range

        CatUnit catUnit = cat.GetComponent<CatUnit>();
        if (nearestEnemy != null)
        {
            catUnit.isAttacking = true;
        }
        else         
        {
            catUnit.isAttacking = false;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CatUnit catUnit = cat.GetComponent<CatUnit>();

        // Only attack if this trigger overlaps an enemy
        if (other.CompareTag("Enemy"))
        {
            Collider2D target = other.GetComponent<Collider2D>();
            if (target != null)
            {
                catUnit.TryAttack(target); // Call Attack component, not static
            }
        }
    }

    void RevealTracker()
    {
        //if there is a target and target is in range. show tracker. else hide tracker.
        if (nearestEnemy != null && inRange)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
