using UnityEngine;

public class CatUnit : MonoBehaviour
{
    public CatData catData;
    public GameObject targetPoint;

    private float attackCooldown;

    [Header("Stats")]
    [SerializeField, Tooltip("Current health of the cat")] private int currentHealth;
    [SerializeField, Tooltip("Attack speed of the cat")] private float attackSpeed;
    [SerializeField, Tooltip("Attack damage of the cat")] private float attackDamage;

    private void Start()
    {
        if (catData != null)
        {
            attackCooldown = 1f / catData.attackSpeed;

            // Set sprite
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr && catData.icon != null)
                sr.sprite = catData.icon;

            currentHealth = catData.health;
            attackSpeed = catData.attackSpeed;
            attackDamage = catData.attackPower;
        }
        else
        {
            Debug.LogWarning("No CatData assigned to " + gameObject.name);
        }

        TriggerTrack triggerTrack = GetComponentInChildren<TriggerTrack>();
        triggerTrack.triggerRadius = catData.attackRange;
    }

    private void Update()
    {
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                Attack(enemy);
                attackCooldown = 1f / attackSpeed;
            }
        }
    }
    public void TryAttack(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                Attack(enemy);
                attackCooldown = 1f / attackSpeed;
            }
        }
    }

    private void Attack(EnemyUnit target)
    {
        Vector3 hitlocation = targetPoint.transform.position;

        target.TakeDamage((int)attackDamage);
        Debug.Log(catData.catName + " attacked " + target.name + " for " + attackDamage + " damage!");
        DamageNumberManager.Instance.ShowDamage((int)attackDamage, hitlocation);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(catData.catName + " has been defeated.");
        Destroy(gameObject);
    }
}
