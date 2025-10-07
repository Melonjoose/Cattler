using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookerUnit : EnemyUnit
{
    [Header("Hooker Exclusive")]
    private LineRenderer line;

    public bool attacking = false;
    private CatUnit hookedCat;
    public float pullSpeed = 2f;

    public float shotCooldown = 10f;
    public float cooldowntimer = 0f;
    public bool lockedCD = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dropLoot = GetComponent<DropLoot>();
        line = GetComponent<LineRenderer>();

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

        //triggerTrack = GetComponentInChildren<EnemyTriggerTrack>();
    }

    // Update is called once per frame
    void Update()
    {

        if (lockedCD == false && hookedCat == null)
        {
            CooldownTimer(); //timer runs
        }

        if(hookedCat == null)  //when theres no target cat
        {
            Walk();
            lockedCD = false; //unlock cooldown
        }
    }


    void DrawLine()
    {
        line.enabled = true;
        line.startColor = Color.red;
        line.endColor = Color.red;

        Vector3 thisUnit = transform.position;
        Vector3 chosenCat = TargetCat.transform.position;
        
        line.positionCount = 2;
        line.SetPosition(0, thisUnit);
        line.SetPosition(1, chosenCat);

    }
    void DeleteLine()
    {
        if (line != null)
        {
            line.enabled = false;
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
    }

    void CooldownTimer()
    {

        cooldowntimer += Time.deltaTime; //make cooldowntimer run.

        if (cooldowntimer > shotCooldown) // if cooldowntimer is bigger than the cooldown
        {
            StartCoroutine(StartAttackSequence());
            cooldowntimer = 0f; // reset cooldowntimer to 0
        }
    }

    IEnumerator StartAttackSequence()
    {
        lockedCD = true; //stop timer from running
        attacking = true;
        ChooseRandomCat();
        canWalk = false;
        // Start arc updater
        StartCoroutine(UpdateLine());

        yield return new WaitForSeconds(0.5f); //  actually wait
        HookCat();
    }

    void Walk() // State 1
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private Coroutine pullCoroutine;

    void HookCat() // State 2
    {
        if (TargetCat != null)
        {
            DrawLine();

            hookedCat = TargetCat.GetComponent<CatUnit>();
            if (hookedCat != null)
            {
                DebuffManager.instance.Stunned(hookedCat.gameObject, 999f); // Stun indefinitely
                Debug.Log($"{hookedCat.name} is hooked and stunned!");
                pullCoroutine = StartCoroutine(pullCat());
            }
        }
    }

    IEnumerator pullCat()
    {
        while (hookedCat != null && Vector3.Distance(hookedCat.transform.position, this.transform.position) > 0.5)
        {
            hookedCat.transform.position = Vector3.MoveTowards(hookedCat.transform.position, transform.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log($"{hookedCat.name} has been pulled to the Hooker!");
    }

    void UnHookCat() // State 3
    {
        DeleteLine();
        if (TargetCat != null)
        {
            hookedCat.catMovement.canWalk = true;
            hookedCat.GetComponent<CatMovement>().enabled = true;
            DebuffManager.instance.RemoveStun(hookedCat.gameObject);
            hookedCat = null;
        }
    }

    IEnumerator UpdateLine()
    {
        while (attacking) // keep drawing as long as attacking is true
        {
            DrawLine();
            yield return null; // update every frame
        }
    }

    public override void Die()
    {
        if( pullCoroutine != null)
        {
            StopCoroutine(pullCoroutine);
        }
        if(hookedCat != null)
        {
            UnHookCat();
            hookedCat = null;
        }

        SpecialEnemySpawner.instance.RemoveSpawnedEnemies(this.gameObject);
        Currency.instance.AddInk(10); // Add ink to currency
        dropLoot.GiveLoot();
        Destroy(gameObject);
    }
}
