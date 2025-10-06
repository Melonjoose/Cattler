using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[RequireComponent(typeof(LineRenderer))]

public class ArtilleryUnit : EnemyUnit
{

    [Header ("Artillery Exclusive")]

    public float height = 20f;
    public int resolution = 20;
    private LineRenderer line;

    List<GameObject> targetLocations = new List<GameObject>();

    public GameObject targetSpot;
    public bool attacking = false;
    public GameObject artilleryShot;


    public float shotCooldown = 10f;
    public float cooldowntimer = 0f;
    public bool lockedCD = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dropLoot = GetComponent<DropLoot>();
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution + 1;


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
        LinkFloors();
    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            Walk();
        }

        if (lockedCD == false)
        {
            CooldownTimer();
        }
    }


    void DrawArc()
    {
        line.enabled = true;
        line.startColor = Color.red;
        Vector3 start = transform.position;
        Vector3 end = targetSpot.transform.position;
        Vector3 mid = (start + end) / 2 + Vector3.up * height; // peak point

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            // Quadratic Bezier curve: Lerp(Lerp(start, mid, t), Lerp(mid, end, t), t)
            Vector3 p1 = Vector3.Lerp(start, mid, t);
            Vector3 p2 = Vector3.Lerp(mid, end, t);
            Vector3 curvePoint = Vector3.Lerp(p1, p2, t);

            line.SetPosition(i, curvePoint);
        }
    }
    void DeleteArc()
    {
        if (line != null)
        {
            line.enabled = false;
        }
    }

    void ChooseRandomTargetLocation() //choose 1 / 5 position to shoot at.
    {
        int randomIndex = Random.Range(0, 5);
        GameObject chosenTarget = targetLocations[randomIndex];
        targetSpot = chosenTarget;
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
        ChooseRandomTargetLocation();
        canWalk = false;
        // Start arc updater
        StartCoroutine(UpdateArc());

        yield return new WaitForSeconds(2f); //  actually wait
        FireArtillery();

        yield return new WaitForSeconds(5f); //  actually wait
        DeleteArc();
        canWalk = true;
        attacking = false; // reset after attack
        lockedCD = false;
    }

    void Walk()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    void FireArtillery()
    {
        GameObject shotGO = Instantiate(artilleryShot, transform.position, transform.rotation);

        ArtyShot shot = shotGO.GetComponent<ArtyShot>();
        if (shot != null)
        {
            shot.unit = this;
            shot.Launch(targetSpot.transform.position, this);
        }
    }

    IEnumerator UpdateArc()
    {
        while (attacking) // keep drawing as long as attacking is true
        {
            DrawArc();
            yield return null; // update every frame
        }
    }

    //1. choose cat from enemyunit. move towards enemy.
    //2. Once in range, a charge up. target lock indicating where it is shooting.
    //3. after 2 seconds. shoots.
    //4. bullet flies into sky.
    //5. bullet falls directly straight down to location of targeted spot.
    //6. if bullet hits target, damage cat. (projectile script. not here.)

    void LinkFloors()
    {
        for (int i = 1; i <= 5; i++) // Example: 5 floors
        {
            string objName = "Floor UI " + i;
            GameObject floor = GameObject.Find(objName);
            if (floor != null)
            {
                targetLocations.Add(floor);
            }
        }
    }

    public override void Die()
    {
        SpecialEnemySpawner.instance.RemoveSpawnedEnemies(this.gameObject); 
        //Debug.Log(enemyData.enemyName + " has been defeated.");
        Currency.instance.AddInk(10); // Add ink to currency
        dropLoot.GiveLoot();
        Destroy(gameObject);
    }
}
