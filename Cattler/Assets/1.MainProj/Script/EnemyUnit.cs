using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    private GameObject thisUnit; // Reference to self for clarity
    private EnemyMovement EnemyMovement;

    private DropLoot dropLoot;

    public EnemyData enemyData; // ScriptableObject with enemy stats

    [Header("Targeting")]
    public GameObject TargetCat; // Reference to current target

    public GameObject targetPoint; // Assign in inspector (child empty GameObject at attack point)

    private float attackCooldown;

    [Header("Stats")]
    [SerializeField] public int currentHealth;
    [SerializeField] public float attackSpeed;
    [SerializeField] public int attackDamage;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange;

    private void Start()
    {
        EnemyMovement = GetComponent<EnemyMovement>();
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

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            CatUnit cat = other.GetComponent<CatUnit>();
            if (cat != null && other.gameObject == TargetCat) // only attack chosen target
            {
                AttackCat(cat);
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
            EnemyMovement.TargetCat = TargetCat;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        //Debug.Log(enemyData.enemyName + " takes " + amount + " damage. Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if(this.CompareTag("Enemy") == true)
        {
            if (EnemySpawner.instance != null) { EnemySpawner.instance.RemoveSpawnedEnemies(thisUnit); } //Clear Spawned Enemies list to prevent targeting dead cats
        }
        if (this.CompareTag("Artillery"))
        {
            if (SpecialEnemySpawner.instance != null) { SpecialEnemySpawner.instance.RemoveSpawnedEnemies(thisUnit); } //Clear Spawned Enemies list to prevent targeting dead cats
        }
        //Debug.Log(enemyData.enemyName + " has been defeated.");
        Currency.instance.AddInk(10); // Add ink to currency
        dropLoot.GiveLoot();
        Destroy(gameObject);
    }


    private void AttackCat(CatUnit cat)
    {
        if (cat == null) return;

        Vector3 hitLocation = targetPoint != null ? targetPoint.transform.position : transform.position;

        DamageNumberManager.Instance.ShowDamage((int)attackDamage, hitLocation);
        cat.TakeDamage((int)attackDamage); // Call CatUnit’s TakeDamage
        
        if (this.gameObject.CompareTag("Artillery") == true) 
            
            return; 
        //Debug.Log(enemyData.enemyName + " attacked " + cat.name + " for " + attackDamage + " damage!");
    }
}
