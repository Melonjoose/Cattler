using Spine.Unity;
using System.Collections;
using UnityEngine;

public class EnemyUnit : Unit
{
    private GameObject thisUnit; // Reference to self for clarity
    public SkeletonAnimation skeletonAnimation;
    private EnemyMovement EnemyMovement;
    public EnemyTriggerTrack triggerTrack;

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
            InitializeFromData(enemyData);
            if(EnemySpawner.instance.currentLevel != null)
            {
                AdjustEnemyDifficulty(EnemySpawner.instance.currentLevel);
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


    protected override void OnUnitUpdate()
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

        if(canWalk && EnemyMovement != null)
        {
            EnemyMovement.enabled = true;
        }
        else if(!canWalk && EnemyMovement != null)
        {
            EnemyMovement.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (attackCooldown <= 0f && canAttack)
        {
            CatUnit cat = other.GetComponent<CatUnit>();
            if (cat != null && other.gameObject == TargetCat && isDead == false) // only attack chosen target. if it's not dead
            {
                AttackCat(cat);
                attackCooldown = 1f / attackSpeed; // Reset cooldown
            }
        }
    }
    public void InitializeFromData(EnemyData data)
    {
        enemyData = data;
        maxHealth = data.health;
        currentHealth = data.health;
        attackSpeed = data.attackSpeed;
        attackDamage = data.attackPower;
        moveSpeed = data.movementSpeed;
        attackRange = data.attackRange;
        // Apply sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr && data.icon != null)
        {
            sr.sprite = data.icon;
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
        if(isDead) return; // Don't take damage if already dead

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


    public void AdjustEnemyDifficulty(Level level)
    {
        Debug.Log("adjust played;");
        if (level == null) return;

        attackDamage = enemyData.attackPower + level.ATKIncrease;
        maxHealth = enemyData.health + level.HPIncrease;
        currentHealth = maxHealth;
        attackSpeed = enemyData.attackSpeed + level.ATKSPDIncrease;
        moveSpeed = enemyData.movementSpeed + level.MVSPDIncrease;

        DropLoot dropLootComponent = GetComponent<DropLoot>();
        float lootMultiplier = 1 + (level.lootPercentage / 100f);
        dropLootComponent.minInkDrop = (int)(dropLootComponent.minInkDrop * lootMultiplier);
        dropLootComponent.maxInkDrop = (int)(dropLootComponent.maxInkDrop * lootMultiplier);

    }
}
