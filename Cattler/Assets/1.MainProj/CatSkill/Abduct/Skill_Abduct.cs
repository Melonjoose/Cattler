using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Abduct : CatSkill
{
    public Rigidbody2D unitRB;

    public Collider2D abductCollider; // the collider for the beam that pulls enemies in. should be a trigger collider that is active during the horizontal movement of the cat.
    public GameObject pullPoint;
    //Pull enemy backwards as UFOcat fly over their head. Enemy takes constant damage in the beam
    private Dictionary<EnemyUnit, float> damageTimers = new Dictionary<EnemyUnit, float>();


    void Start()
    {
        catUnit = GetComponentInParent<CatUnit>();
        unitRB = catUnit.GetComponent<Rigidbody2D>();
        abductCollider = this.GetComponent<Collider2D>();
    }

    public override void UseSkill()
    {
        StartCoroutine(UFOAbductSequence());
    }


    IEnumerator UFOAbductSequence()
    {
        SkillState();
        yield return new WaitForSeconds(0.3f);
        AnimationState();
        //movement sequence
        //3. the cat flies up abit then flies horiztontally right across the screen.
            Vector3 startPosition = catUnit.transform.position;
            Vector3 upPosition = startPosition + new Vector3(0, 0.5f, 0); // fly up by 2 units
        //move with lerp to the up position over 0.2 seconds
            float elapsedTime = 0f;
            float duration = 0.2f;
            while (elapsedTime < duration)
            {
                catUnit.transform.position = Vector3.Lerp(startPosition, upPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            catUnit.transform.position = upPosition; // ensure it ends at the exact up position

        
        yield return new WaitForSeconds(0.2f);
            Vector3 RightPosition = startPosition + new Vector3(25f, 0.5f, 0); // fly right across the screen by 10 units
        //move with lerp to the end position over 2 second
            elapsedTime = 0f;
            duration = 2f;
            while (elapsedTime < duration)
            {
                catUnit.transform.position = Vector3.Lerp(upPosition, RightPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            catUnit.transform.position = RightPosition; // ensure it ends at the exact end position

        //once it reaches the end of the screen, turns of the collider beam that pulls enemies in. then fly up.
        abductCollider.enabled = false; //enable the collider for the beam that pulls enemies in during the skill

        yield return new WaitForSeconds(0.2f);
        Vector3 upPosition2 = RightPosition + new Vector3(0, 8f, 0); // fly up by 2 units
        //move with lerp to the up position over 0.2 seconds
        elapsedTime = 0f;
        duration = 0.5f;
        while (elapsedTime < duration)
        {
            catUnit.transform.position = Vector3.Lerp(RightPosition, upPosition2, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        catUnit.transform.position = upPosition2; // ensure it ends at the exact up position

        yield return new WaitForSeconds(0.2f);
        Vector3 SkyPosition = upPosition2 + new Vector3(-25f, 2f, 0);
        elapsedTime = 0f;
        duration = 0.5f;
        while (elapsedTime < duration)
        {
            catUnit.transform.position = Vector3.Lerp(upPosition2, SkyPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        catUnit.transform.position = SkyPosition; // ensure it ends at the exact up position

        yield return new WaitForSeconds(0.2f);
        elapsedTime = 0f;
        duration = 1f;
        while (elapsedTime < duration)
        {
            catUnit.transform.position = Vector3.Lerp(SkyPosition, startPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        catUnit.transform.position = startPosition; // ensure it ends at the exact up position

        EndSkillState();
    }
    //sequence of events:
    //0. timer runs out, skill is activated. stop the timer until the whole sequence of events is complete. the timer should only start again once the cat has landed back on the ground after flying up and across the screen.
    //1. change the ufo cats animation to the skill1 animation beam is coming below the ufo ship.
    //2. the cat is made invincible and cannot attack or move for the duration of the skill/ cannot be interacted with by the player.
    //3. the cat flies up abit then flies horiztontally right across the screen.
    //4. the abduct has a collider that is active during the horizontal movement of the cat. any enemy that comes into contact with the collider takes damage and is pulled towards the pullpoint.
    //5.the pullpoint is the centre of the beam. a gameobject that I can move around to control where the enemies are pulled towards. the pullpoint should be in front of the cat as it moves across the screen, so that enemies are pulled towards the front of the beam.
    //6. cat flies off the screen, flies up above the screen, then flies back down to it's original position.
    //7. once landed, the timer cooldown starts again. and cat is no longer invincible and can be interacted with by the player again.

    void SkillState() //0.
    {
        isActive = false; //stop timer
        catUnit.canAttack = false; //cat cannot attack during the skill
        catUnit.canWalk = false; //cat cannot move during the skill
        catUnit.canTarget = false; //cat cannot be targeted by the enemyunits during the skill
        unitRB.bodyType = RigidbodyType2D.Kinematic; //make the cat not affected by physics during the skill
        abductCollider.enabled = true; //enable the collider for the beam that pulls enemies in during the skill
    }

    void AnimationState()
    {
        catUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Skill1", true); //1. change animation to skill animation
    }

    //currently the UFO is not invicible and still can be hit.
    //the cat is moving to the right too fast.
    //damage tick is too fast.

    void EndSkillState() //7.
    {
        isActive = true; //start timer again
        catUnit.canAttack = true; //cat can attack again after the skill
        catUnit.canWalk = true; //cat can move again after the skill
        catUnit.canTarget = true; //cat can be targeted by the enemyunits again after the skill
        unitRB.bodyType = RigidbodyType2D.Dynamic; //make the cat not affected by physics during the skill
        abductCollider.enabled = false; //enable the collider for the beam that pulls enemies in during the skill

    }

    void OnTriggerStay2D(Collider2D other)
    {
        EnemyUnit enemy = other.GetComponent<EnemyUnit>();
        if (enemy != null)
        {
            // Check if we have a timer for this enemy
            if (!damageTimers.ContainsKey(enemy))
                damageTimers[enemy] = 0f;

            // If enough time has passed, apply damage
            if (Time.time - damageTimers[enemy] >= 1f)
            {
                enemy.TakeDamage(1); // 1 damage every second
                damageTimers[enemy] = Time.time; // reset timer
            }


            // Pull towards pullPoint
            Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
            if (enemyRB != null)
            {
                Vector2 direction = (pullPoint.transform.position - enemy.transform.position).normalized;
                float distance = Vector2.Distance(enemy.transform.position, pullPoint.transform.position);
                float pullForce = Mathf.Lerp(20f, 5f, distance / 10f); // stronger when closer
                enemyRB.AddForce(direction * pullForce);

            }
            else
            {
                // fallback if no Rigidbody2D
                Vector3 direction = (pullPoint.transform.position - enemy.transform.position).normalized;
                float distance = Vector2.Distance(enemy.transform.position, pullPoint.transform.position);
                float pullForce = Mathf.Lerp(20f, 5f, distance / 10f); // stronger when closer
                enemyRB.AddForce(direction * pullForce);

            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        EnemyUnit enemy = other.GetComponent<EnemyUnit>();
        if (enemy != null)
        {
            // Clean up when enemy leaves the beam
            if (damageTimers.ContainsKey(enemy))
                damageTimers.Remove(enemy);
        }
    }

}
