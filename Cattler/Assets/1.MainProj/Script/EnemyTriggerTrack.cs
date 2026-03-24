using UnityEngine;

public class EnemyTriggerTrack : MonoBehaviour
{
    private GameObject enemy;
    public float triggerRadius = 5f;
    public bool inRange = false;
    public float moveSpeed = 3f;
    public Vector3 offsetFromEnemy = new Vector3(1f, 0, 0);
    public bool onTarget = false;

    public GameObject chosenCat;
    private EnemyUnit enemyUnit;

    void Start()
    {
        enemy = transform.parent.gameObject;
        enemyUnit = enemy.GetComponent<EnemyUnit>();
        triggerRadius = enemyUnit.attackRange;
    }

    void FixedUpdate()
    {
        // Update range check first
        inRange = IsInRange();

        TrackEnemy();
    }

    void TrackEnemy()
    {
        if (chosenCat != null && inRange)
        {
            // Move toward the cat
            transform.position = Vector3.MoveTowards(
                transform.position,
                chosenCat.transform.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            // Return to offset position near enemy
            Vector3 targetPosition = enemy.transform.position + offsetFromEnemy;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }

        OnTargetChecker();
    }

    bool IsInRange()
    {
        if (chosenCat == null) return false;

        float distance = Vector3.Distance(enemy.transform.position, chosenCat.transform.position);
        return distance <= triggerRadius;
    }

    void OnTargetChecker()
    {
        if (enemy == null || chosenCat == null) return;

        // Check if tracker has reached the cat
        onTarget = (transform.position == chosenCat.transform.position);
    }
}