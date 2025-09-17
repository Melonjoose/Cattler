using UnityEngine;

public class ArtilleryUnit : MonoBehaviour
{
    private GameObject thisUnit; // Reference to self for clarity

    private DropLoot dropLoot;

    public EnemyData enemyData; // ScriptableObject with enemy stats
    [Header("Targeting")]
    public GameObject TargetCat; // Reference to current target

    public GameObject targetPoint; // Assign in inspector (child empty GameObject at attack point)

    private float attackCooldown;

    [Header("Stats")]
    [SerializeField] public int currentHealth;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackDamage;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange;

    private void Start()
    {
        thisUnit = this.gameObject;
        dropLoot = GetComponent<DropLoot>();

        if (enemyData != null)
        {
            // Initialize stats from SO
            currentHealth = enemyData.health;
            attackSpeed = enemyData.attackSpeed;
            attackDamage = enemyData.attackPower;
            moveSpeed = enemyData.movementSpeed;
            attackRange = enemyData.attackRange;

            // Apply sprite
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr && enemyData.icon != null)
            {
                sr.sprite = enemyData.icon;
            }
        }
        else
        {
            Debug.LogWarning("No EnemyData assigned to " + gameObject.name);
        }

        LinkTargetpoint(); // Link the targetPoint to an object called targetPoint located in this enemy's children
        EnemyTriggerTrack triggerTrack = GetComponentInChildren<EnemyTriggerTrack>();
        triggerTrack.triggerRadius = enemyData.attackRange;
    }


    private void Update()
    {
        // Tick down cooldown
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        if (TargetCat == null)
        {
            ChooseRandomCat();
        }
        else
        {
            MovetowardsCat();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            CatUnit cat = other.GetComponent<CatUnit>();
            if (cat != null && other.gameObject == TargetCat) // only attack chosen target
            {
                LockOnCat(cat);
                attackCooldown = 1f / attackSpeed; // Reset cooldown
            }
        }
    }

    void LinkTargetpoint()
    {
        Transform tp = transform.Find("TargetPoint");
        if (tp != null)
        {
            targetPoint = tp.gameObject;
        }
        else
        {
            Debug.LogWarning("No child named 'targetPoint' found under " + gameObject.name);
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
        if (EnemySpawner.instance != null) { EnemySpawner.instance.RemoveSpawnedEnemies(thisUnit); } //Clear Spawned Enemies list to prevent targeting dead cats
        Debug.Log(enemyData.enemyName + " has been defeated.");
        Currency.instance.AddInk(10); // Add ink to currency
        dropLoot.GiveLoot();
        Destroy(gameObject);
    }

    private void MovetowardsCat()
    {
        if (TargetCat == null) return;

        Vector3 direction = (TargetCat.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void ChooseRandomCat()
    {
        GameObject[] allCats = GameObject.FindGameObjectsWithTag("Cat");

        if (allCats.Length == 0)
        {
            Debug.LogWarning("No cats found in the scene.");
            TargetCat = null;
            return;
        }

        // Pick random
        TargetCat = allCats[Random.Range(0, allCats.Length)];

        if (TargetCat != null)
        {
            //Debug.Log("Target Cat: " + TargetCat.name);
        }
    }

    private void LockOnCat(CatUnit cat)
    {
        if (cat == null) return;

        Vector3 hitLocation = targetPoint != null ? targetPoint.transform.position : transform.position;

        //artillery stops moving.

        //instantiate a lock on effect on the cat's position

        //once target is locked on cat, shoot projectile in air and out of screen.

        //after firing, move towards cats.

        //projectile move to above the cat's position outside of screen

        //projectile to be positioned at targetPoint's X position, Y + some height, Z 0.

        //wait for a few seconds for player to react.

        //projectile moves down to hitLocation

        //projectile collides with cat, dealing damage

        //after cooldown ends, artillery stop moving, locks on and shoots again.
    }
}
