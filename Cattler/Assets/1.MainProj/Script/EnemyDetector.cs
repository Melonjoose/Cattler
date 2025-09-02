using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public static EnemyDetector instance;
    public BoxCollider2D detectionArea;
    public bool enemyDetected = false;

    [SerializeField]private int enemyCount = 0; // number of enemies inside

    void Start()
    {
        instance = this;
        detectionArea = GetComponent<BoxCollider2D>();
        detectionArea.isTrigger = true; // make sure it's a trigger
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyCount++;
            enemyDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                enemyDetected = false;
                enemyCount = 0; // safety
            }
        }
    }
}
