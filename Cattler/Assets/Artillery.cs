using NUnit.Framework;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[RequireComponent(typeof(LineRenderer))]

public class Artillery : MonoBehaviour
{
    public EnemyUnit unit;
    public EnemyMovement movement;
    public EnemyTriggerTrack triggerTrack;

    public float height = 20f;
    public int resolution = 20;
    private LineRenderer line;

    public GameObject[] targetLocations;

    public GameObject targetSpot;
    public bool attacking = false;
    public GameObject artilleryShot;
    public float moveSpeed = 3;
    public bool canWalk = true;

    public float shotCooldown = 10f;
    public float cooldowntimer = 0f;
    public bool lockedCD = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution + 1;

        unit = GetComponent<EnemyUnit>();
        movement = GetComponent<EnemyMovement>();
        //triggerTrack = GetComponentInChildren<EnemyTriggerTrack>();
        //targetLocations = FindObjectByName("Floor UI "[0].name); // Add Floor UI 1, Floor UI 2, etc to list. **
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
        Vector3 start = unit.transform.position;
        Vector3 end = targetSpot.transform.position;
        Vector3 mid = (start + end) / 2 + Vector3.up * height; // peak point

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            // Quadratic Bezier curve: Lerp(Lerp(start, mid, t), Lerp(mid, end, t), t)
            Vector3 p1 = Vector3.Lerp(start, mid, t);
            Vector3 p2 = Vector3.Lerp(mid, end, t);
            Vector3 curvePoint = Vector3.Lerp(p1, p2, t);

            line.startColor = Color.red;
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
            shot.unit = unit;
            shot.Launch(targetSpot.transform.position, unit);
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
}
