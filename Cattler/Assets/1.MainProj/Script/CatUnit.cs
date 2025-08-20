using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class CatUnit : MonoBehaviour
{
    public GameObject targetPoint;
    public CatData baseData;
    public CatRuntimeData runtimeData;

    public int currentHealth;
    public int currentEXP;
    public float attackPower;
    

    private float attackCooldown;

    private void Start()
    {

        if (baseData != null)
        {
            runtimeData = new CatRuntimeData(baseData);
            GetComponent<SpriteRenderer>().sprite = runtimeData.icon;
        }
           
        else
        {
            Debug.LogWarning("No CatData assigned to " + gameObject.name);
        }


        LinkTargetpoint();//link the targetPoint to and object called targetPoint located in this enemy's children
        TriggerTrack triggerTrack = GetComponentInChildren<TriggerTrack>();
        triggerTrack.triggerRadius = runtimeData.attackRange;
    }

    private void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime; //reset cooldown if not attacking
        }

        currentHealth = runtimeData.currentHealth;
        attackPower = runtimeData.attackPower;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                Attack(enemy);
                attackCooldown = 1f / runtimeData.attackSpeed;
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
    public void TryAttack(Collider2D other)
    {
        if (attackCooldown <= 0f)
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                Attack(enemy);
                attackCooldown = 1f / runtimeData.attackSpeed;
            }
        }
    }

    private void Attack(EnemyUnit target)
    {
        Vector3 hitlocation = targetPoint.transform.position; // Indicate the targetPoint's position for where the damage number will be displayed

        target.TakeDamage((int)runtimeData.attackPower); //run the TakeDamage method on the target enemy by dealing attackPower damage. (TAKES ACTUAL DMG)
        Debug.Log(runtimeData.catName + " attacked " + target.name + " for " + runtimeData.attackPower + " damage!"); //debug to state damage dealt to who in console
        DamageNumberManager.Instance.ShowDamage((int)runtimeData.attackPower, hitlocation); //Showdamage at location (SHOWS DMG TAKEN)
    }


    public void TakeDamage(int amount)
    {
        runtimeData.currentHealth -= amount;
        if (runtimeData.currentHealth <= 0)
        {
            Die();
        }
    } //runs when triggered by ICombatUnit.cs

    private void Die()
    {
        Debug.Log(runtimeData.catName + " has been defeated.");
        Destroy(gameObject);
        //Give Send EXP gained from death to Retreat controller   
    }
}
