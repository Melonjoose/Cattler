
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CatUnit : MonoBehaviour
{
    public GameObject targetPoint;

    public CatRuntimeData runtimeData;
    public CatMovement catMovement;

    private float attackCooldown;

    public bool isAttacking = false;
    public bool isStunned = false;

    public bool canAttack = true;

    private void Start()
    {   
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
        else
        {
            Debug.LogWarning("No child named 'targetPoint' found under " + gameObject.name);
        }
    }
    
    public void TryAttack(Collider2D other)
    {
        EnemyUnit enemytarget = other.GetComponent<EnemyUnit>();

        if (attackCooldown <= 0f)
        {
            
            if (enemytarget != null)
            {
                Attack(enemytarget);
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
    }


    public void TakeDamage(int amount)
    {
        runtimeData.currentHealth -= amount;
        int dyingHealth = runtimeData.maxHealth / 3;
        if (runtimeData.currentHealth <= 0)
        {
            Die();
        }
        else if (runtimeData.currentHealth <= dyingHealth)         //33% of max health
        {
            Dying();
        }
    } 

    public void Dying()
    {
        CommentaryManager.instance.AddDialogueToQueue(1); // Cat defeated dialogue
    }

    private void Die()
    {   
        CommentaryManager.instance.AddDialogueToQueue(2); // Cat defeated dialogue
        Debug.Log(runtimeData.template.itemName + " has been defeated.");
        Destroy(gameObject);
        //Give Send EXP gained from death to Retreat controller   
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
