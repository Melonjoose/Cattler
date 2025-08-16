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

        TriggerTrack triggerTrack = GetComponentInChildren<TriggerTrack>();
        triggerTrack.triggerRadius = runtimeData.attackRange;
    }

    private void Update()
    {
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        currentHealth = runtimeData.currentHealth;
        attackPower = runtimeData.attackPower;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("TriggerStay2D: " + other.name);
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
        Vector3 hitlocation = targetPoint.transform.position;

        target.TakeDamage((int)runtimeData.attackPower);
        Debug.Log(runtimeData.catName + " attacked " + target.name + " for " + runtimeData.attackPower + " damage!");
        DamageNumberManager.Instance.ShowDamage((int)runtimeData.attackPower, hitlocation);
    }


    public void TakeDamage(int amount)
    {
        runtimeData.currentHealth -= amount;
        if (runtimeData.currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(runtimeData.catName + " has been defeated.");
        Destroy(gameObject);
    }
}
