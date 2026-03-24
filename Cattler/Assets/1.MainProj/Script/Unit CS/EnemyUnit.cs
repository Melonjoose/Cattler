using Spine.Unity;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    private GameObject thisUnit; // Reference to self for clarity
    public SkeletonAnimation skeletonAnimation;
    private EnemyMovement EnemyMovement;
    private EnemyTriggerTrack triggerTrack;
    public bool canWalk = true;
    public bool isDead = false;

    public bool pickRandomCat = false;
    public bool pickClosestCat = true;

    public DropLoot dropLoot;

    public EnemyData enemyData; // ScriptableObject with enemy stats

    [Header("Targeting")]
    public GameObject TargetCat; // Reference to current target

    public GameObject targetPoint; // Assign in inspector (child empty GameObject at attack point)

    private float attackCooldown;

    [Header("Stats")]
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public float attackSpeed;
    [SerializeField] public int attackDamage;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange;

    private void Start()
    {   
        
        EnemyMovement = GetComponent<EnemyMovement>();
        thisUnit = this.gameObject;

        //apply skeletonanimation
        Transform child = transform.Find("Spine GameObject");
        if (child != null)
        {
            skeletonAnimation = child.GetComponent<SkeletonAnimation>();
        }

        skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        
        dropLoot = GetComponent<DropLoot>();

        if (enemyData != null)
        {
            // Initialize stats from SO
            maxHealth = enemyData.health;
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
        triggerTrack = GetComponentInChildren<EnemyTriggerTrack>();
        triggerTrack.triggerRadius = enemyData.attackRange;
    }


    private void Update()
    {
        // Tick down cooldown
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        if (TargetCat == null || !TargetCat.activeSelf)
        {
            if(pickRandomCat == true)
            {
                ChooseRandomCat();
            }
            if (pickClosestCat == true)
            {
                FindClosestCat();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            CatUnit cat = other.GetComponent<CatUnit>();
            if (cat != null && other.gameObject == TargetCat && isDead == false) // only attack chosen target. if it's not dead
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
    void FindClosestCat()
    {
        GameObject[] allCats = GameObject.FindGameObjectsWithTag("Cat");

        float closestDistance = Mathf.Infinity;
        GameObject closestCat = null;

        foreach (GameObject cat in allCats)
        {
            float distance = Vector3.Distance(this.transform.position, cat.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCat = cat;
            }
        }

        TargetCat = closestCat; // Will be null if no enemies are in range
        triggerTrack.chosenCat = closestCat;
        EnemyMovement.TargetCat = closestCat;
    }

    private void ChooseRandomCat()
    {
        if (this.CompareTag("Artillery"))
            return;

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
            triggerTrack.chosenCat = TargetCat;
        }
    }

    public void TakeDamage(int amount)
    {
        StatFXManager.instance.PlayVFX(this.transform.position, 1); // onhit vfx
        AudioManager.instance.PlaySFX("EnemyHit");
        currentHealth -= amount;
        
        skeletonAnimation.AnimationState.SetAnimation(0, "Hit", false).Complete += (trackEntry) =>
        {
            if (canWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
            }
        };

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
            skeletonAnimation.AnimationState.SetAnimation(0, "Death", false).Complete += (trackEntry) =>
            {         
                Destroy(gameObject);
            };

        }
    }

    public virtual void Die()
    {
        canWalk = false;
        StopAllCoroutines();
        StatFXManager.instance.PlayVFX(this.transform.position, 0);
        StatFXManager.instance.PlayVFX(this.transform.position, 2);
        AudioManager.instance.PlaySFX("EnemyDie");

        EnemySpawner.instance.RemoveSpawnedEnemies(thisUnit); 
        dropLoot.GiveLoot();

        EnemyDetector.instance.OnEnemyDestroyed(gameObject);
    }


    private void AttackCat(CatUnit cat)
    {
        if (cat == null) return;

        Vector3 hitLocation = targetPoint != null ? targetPoint.transform.position : transform.position;

        DamageNumberManager.Instance.ShowDamage((int)attackDamage, hitLocation);
        cat.TakeDamage((int)attackDamage); // Call CatUnit’s TakeDamage
        
        skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false).Complete += (trackEntry) =>
        {
            if (canWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
            }
        };

        if (this.gameObject.CompareTag("Artillery") == true) 
            
            return; 
        //Debug.Log(enemyData.enemyName + " attacked " + cat.name + " for " + attackDamage + " damage!");
    }
}
