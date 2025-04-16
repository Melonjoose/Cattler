using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public EnemyData enemyData; // Drag in your ScriptableObject (e.g., BasicEnemy.asset)

    [Header("Stats")]
    [SerializeField, Tooltip("Current health of the Enemy")] private int currentHealth;
    [SerializeField, Tooltip("Attack speed of the Enemy")] private float attackSpeed;
    [SerializeField, Tooltip("Attack damage of the Enemy")] private float attackDamage;

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
        }
        else
        {
            Debug.LogWarning("No EnemyData assigned to " + gameObject.name);
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
}
