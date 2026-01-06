using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CatUnit : MonoBehaviour
{
    //--- Equipments ---// memory allocation
    public Item hat;
    public Item weaponL; //L
    public Item weaponR; //R

    //--- Cat linked objects ---//
    public GameObject catGO;
    public GameObject targetPoint;

    public CatRuntimeData runtimeData;
    public CatMovement catMovement;
    public CatIconUI.CatIconSlot catIconSlot;

    private float attackCooldown;

    public bool isAttacking = false;
    public bool isStunned = false;
    public bool isDead = false;

    public bool canAttack = true;

    public event Action<int,int> onHealthChanged;
    public event Action CatDeath;

    private void Start()
    {   
        InventoryIcon item = this.GetComponent<InventoryIcon>();
        if(item == null)
        {
            catGO = this.gameObject;
        }
        catMovement = GetComponent<CatMovement>();
        LinkTargetpoint();//link the targetPoint to and object called targetPoint located in this enemy's children
    }

    private void Update()
    {
        if (canAttack) 
        {
            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime; //reset cooldown if not attacking
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryAttack(other);
    }

    void LinkTargetpoint()
    {
        Transform tp = transform.Find("TargetPoint");
        if (tp != null)
        {
            targetPoint = tp.gameObject;
        }
    }
    
    public void TryAttack(Collider2D other)
    {
        if(targetPoint == null)
        {
            Debug.LogWarning($"{gameObject.name} do not have a 'targetPoint' and is unable to attack");
            return;
        }

        EnemyUnit enemytarget = other.GetComponent<EnemyUnit>();
        if (attackCooldown <= 0f)
        {
            
            if (enemytarget != null)
            {
                Attack(enemytarget);
                Knockback(enemytarget); //knockback effect when attacked
                attackCooldown = 1f / runtimeData.attackSpeed;
            }
        }
    }

    private void Attack(EnemyUnit target)
    {
        Vector3 hitlocation = targetPoint.transform.position; // Indicate the targetPoint's position for where the damage number will be displayed

        target.TakeDamage((int)runtimeData.attackPower); //run the TakeDamage method on the target enemy by dealing attackPower damage. (TAKES ACTUAL DMG)
        //Debug.Log(runtimeData.unitName + " attacked " + target.name + " for " + runtimeData.attackPower + " damage!"); //debug to state damage dealt to who in console

        DamageNumberManager.Instance.ShowDamage((int)runtimeData.attackPower, hitlocation); //Showdamage at location (SHOWS DMG TAKEN)
        //VFX can be added here later.
    }

    private void Knockback(EnemyUnit target)
    {
        Rigidbody2D targetrb = target.GetComponent<Rigidbody2D>();
        if (targetrb != null)
        {
            Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;
            float knockbackForce = 3f; // Adjust force as needed
            targetrb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int amount)
    {

        runtimeData.currentHealth -= amount;
        onHealthChanged?.Invoke(runtimeData.currentHealth , runtimeData.maxHealth);
        int dyingHealth = runtimeData.maxHealth / 3;
        if (runtimeData.currentHealth <= 0)
        {
            Die();
        }
        else if (runtimeData.currentHealth <= dyingHealth)         //33% of max health
        {
            Dying();
            //Add a saving Grace function here later. ensure it survives at 1HP instead of dying.
            //Invunerable for 2 seconds.
            //then add a knockback to all enemies around it.
        }
    } 

    public void OnHealthChange(int currentHPAmount , int maxHpAmount)
    {
        //onHealthChanged?.Invoke(runtimeData.currentHealth , runtimeData.maxHealth);
        onHealthChanged?.Invoke(currentHPAmount, maxHpAmount);
    }

    public void Dying()
    {
        CommentaryManager.instance.AddDialogueToQueue(1); // Cat defeated dialogue
    }

    private void Die()
    {   
        CommentaryManager.instance.AddDialogueToQueue(2); // Cat defeated dialogue
        Debug.Log(runtimeData.template.itemName + " has been defeated.");
        //Give Send EXP gained from death to Retreat controller   
        CatDeath?.Invoke();
        
        isDead = true;

        CatIconUI.instance.SetIconToDead(catIconSlot);

        TeamManager.instance.StoreToDeadCatsList(this);
        
        catGO.SetActive(false);
        //Destroy(gameObject);  //delete when return to lobby.
    }

    public void AssignCat(CatRuntimeData runtimeCat)
    {
        runtimeData = runtimeCat;
        GetComponent<SpriteRenderer>().sprite = runtimeCat.template.icon;
    }

    private Coroutine stunCoroutine;

    public void Stunned(float duration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine); //stops the current stun
        }

        //reapply stun
        isStunned = true; //isstunned
        canAttack = false; //cannot attack
        this.GetComponent<CatMovement>().enabled = false; //cannot move
        stunCoroutine = StartCoroutine(StunSequence(duration));
    }

    IEnumerator StunSequence(float duration)
    {
        yield return new WaitForSeconds(duration);
        Unstun();
    }

    public void Unstun()
    {
        if (!isStunned) return; // if not stunned, then cannot be unstunned.

        isStunned = false; //notstunned
        canAttack = true; //can attack
        this.GetComponent<CatMovement>().enabled = true; //can move

        stunCoroutine = null;
    }


}
