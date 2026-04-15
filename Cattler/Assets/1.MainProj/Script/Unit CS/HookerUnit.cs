using Spine.Unity;
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
    public float hookduration = 7f;

    public float shotCooldown = 10f;
    public float cooldowntimer = 0f;
    public bool lockedCD = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnUnitStart()
    {
        dropLoot = GetComponent<DropLoot>();
        line = GetComponent<LineRenderer>();

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

        //apply skeletonanimation
        Transform child = transform.Find("Spine GameObject");
        if (child != null)
        {
            skeletonAnimation = child.GetComponent<SkeletonAnimation>();
        }

        skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
    }

    // Update is called once per frame
    protected override void OnUnitUpdate()
    {

        if(isStunned)
        {
            bool playedOnce = false;
            if (playedOnce)
            {
                return;
            }
            UnHookCat();
            skeletonAnimation.AnimationState.SetAnimation(0, "Hit", false);
            return;
        }

        if (lockedCD == false && hookedCat == null && canAttack)
        {
            CooldownTimer(); //timer runs
        }

        if(hookedCat == null)  //when theres no target cat
        {
            Walk();
            lockedCD = false; //unlock cooldown
        }
    }
    private void PlayAnimation(string animName, bool loop = false)
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animName, loop);
        }
    }


    void DrawLine()
    {
        if (TargetCat == null) return;

        line.enabled = true;
        line.startColor = Color.red;
        line.endColor = Color.red;

        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, TargetCat.transform.position);
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
            if (isDead)
            {
                return; //safeguard when this unit is dead, it cannot start a new attack
            }
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
        PlayAnimation("AttackStart", false);
        HookCat();
    }

    void Walk() //state 1
    {
        if (canWalk)
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }


    private Coroutine pullCoroutine;

    void HookCat()
    {
        if (TargetCat != null)
        {
            DrawLine();
            hookedCat = TargetCat.GetComponent<CatUnit>();
            if (hookedCat != null)
            {
                DebuffManager.instance.ApplyDebuff(hookedCat.gameObject, DebuffManager.instance.stun , hookduration);
                Debug.Log($"{hookedCat.name} is hooked and stunned!");
                PlayAnimation("AttackLoop", true);
                pullCoroutine = StartCoroutine(pullCat());
            }
        }
    }

    IEnumerator pullCat()
    {
        float timeout = hookduration; // max seconds to pull
        float elapsed = 0f;

        while (hookedCat != null
               && Vector3.Distance(hookedCat.transform.position, transform.position) > 1.2f
               && hookedCat.isDead == false
               && elapsed < timeout)
        {
            hookedCat.transform.position = Vector3.MoveTowards(
                hookedCat.transform.position,
                transform.position,
                pullSpeed * Time.deltaTime
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        UnHookCat(); // cleanup
    }


    public void UnHookCat()
    {
        DeleteLine();
        if (hookedCat != null)
        {
            hookedCat.catMovement.canWalk = true;
            hookedCat.GetComponent<CatMovement>().enabled = true;
            DebuffManager.instance.RemoveDebuff(hookedCat.gameObject, DebuffManager.instance.stun);

        }

        hookedCat = null;
        TargetCat = null;
        attacking = false;
        canWalk = true;
        PlayAnimation("Walk", true);
    }


    IEnumerator UpdateLine()
    {
        while (attacking) // keep drawing as long as attacking is true
        {
            DrawLine();
            yield return null; // update every frame
        }
    }

    public override void TakeDamage(Unit hitter, int damage, float knockback)
    {
        if (isDead) return; // Don't take damage if already dead

        base.TakeDamage(hitter, damage, knockback); // Call base method for knockback

        StatFXManager.instance.PlayVFX(this.transform.position, 1); // onhit vfx
        AudioManager.instance.PlaySFX("EnemyHit");
        currentHealth -= damage;

        skeletonAnimation.AnimationState.SetAnimation(0, "Hit", false).Complete += (trackEntry) =>
        {
            if (attacking)
            {
                UnHookCat();
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

        canWalk = false;

        StatFXManager.instance.PlayVFX(this.transform.position, 0);
        StatFXManager.instance.PlayVFX(this.transform.position, 2);
        AudioManager.instance.PlaySFX("EnemyDie");

        dropLoot.GiveLoot();
        SpecialEnemySpawner.instance.RemoveSpawnedEnemies(gameObject);
        EnemyDetector.instance.OnEnemyDestroyed(gameObject);
    }
}
