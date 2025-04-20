using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public EnemyData enemyData; // Drag in your ScriptableObject (e.g., BasicEnemy.asset)
    private GameObject TargetCat;

    [Header("Stats")]
    [SerializeField, Tooltip("Current health of the Enemy")] private int currentHealth;
    [SerializeField, Tooltip("Attack speed of the Enemy")] private float attackSpeed;
    [SerializeField, Tooltip("Attack damage of the Enemy")] private float attackDamage;
    [SerializeField, Tooltip("Movement speed of the Enemy")] private float moveSpeed;
    [SerializeField, Tooltip("Attack range of the Enemy")] private float attackRange;

    private void Start()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.health;

            // Set the sprite visually
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr && enemyData.icon != null)
            {
                sr.sprite = enemyData.icon;
            }

            currentHealth = enemyData.health;
            attackSpeed = enemyData.attackSpeed;
            attackDamage = enemyData.attackPower;
            moveSpeed = enemyData.moveSpeed;
            attackRange = enemyData.attackRange;
        }
        else
        {
            Debug.LogWarning("No EnemyData assigned to " + gameObject.name);
        }
    }

    private void Update()
    {
        if (TargetCat == null)
        {
            ChooseRandomCat();
        }
        else
        {
            MovetowardsCat(TargetCat);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(enemyData.enemyName + " takes " + amount + " damage. Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(enemyData.enemyName + " has been defeated.");
        Destroy(gameObject);
    }

    private void MovetowardsCat(GameObject TargetCat)
    {
        // Move towards the cat
        Vector3 direction = (TargetCat.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void ChooseRandomCat()
    {
        GameObject[] AllCats = GameObject.FindGameObjectsWithTag("Cat"); // Fixed: Correct method to find multiple objects with a tag

        if(AllCats.Length == 0)
        {
            Debug.LogWarning("No cats found in the scene.");
            GameObject TargetCat = null;
            return;
        }
        
        foreach (GameObject cat in AllCats) // Use the array of cats
        {
            if (Random.Range(0, 4) == 0)
            {
                TargetCat = cat; // Fixed: Correct variable name
            }
        }

        if (TargetCat != null)
        {
            Debug.Log("Target Cat: " + TargetCat.name);
            MovetowardsCat(TargetCat);
        }
        else
        {
            Debug.LogWarning("No target cat found.");
        }
    }

}
