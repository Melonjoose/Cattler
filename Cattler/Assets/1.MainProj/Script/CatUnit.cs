using Spine;
using Spine.Unity;
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
    public CatUnit thisCatUnit;
    public InventoryIcon inventoryIcon; //this cat's UI icon in the inventory.
    public GameObject catGO; //world cat gameobject
    public GameObject catLobby; // reference the lobby cat of this
    public GameObject targetPoint;
    public GameObject AnimationBody; //the gameobject that has the animator component for this cat. (for animation purposes only, not the actual catGO)
    public SkeletonAnimation skeletonAnimation;
    public SkeletonGraphic skeletonGraphic;

    public CatRuntimeData runtimeData;
    public CatMovement catMovement;
    public CatIconUI.CatIconSlot catIconSlot;
    public string catSkin;

    private float attackCooldown;

    public bool isAttacking = false;
    public bool isStunned = false;
    public bool isDead = false;

    public bool canAttack = true;

    public event Action<int,int> onHealthChanged;
    public event Action CatDeath;

    private void Start()
    {
        thisCatUnit = GetComponent<CatUnit>();
        inventoryIcon = GetComponent<InventoryIcon>();

        if (inventoryIcon == null)
            catGO = gameObject;

        catMovement = GetComponent<CatMovement>();
        LinkTargetpoint();
        LinkAnimationBody();

        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
            TravelManager.instance.OnTravelStateChanged += HandleTravelStateChanged; // only subscribe if this catUnit.cs have a skeletonAnimation
            HandleTravelStateChanged(TravelManager.instance.IsTraveling);
        }

        AnimationLogic();

        if (runtimeData != null && runtimeData.template != null)
            catSkin = runtimeData.template.skinName;
    }

    private void OnDestroy()
    {
        if (skeletonAnimation != null)
            skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;

        if (TravelManager.instance != null)
            TravelManager.instance.OnTravelStateChanged -= HandleTravelStateChanged;
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

    public void LinkAnimationBody()
    {
        Transform found = transform.Find("Spine GameObject (Cat)");
        if (found == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have an AnimationBody linked and is unable to play animations.");
            return;
        }

        AnimationBody = found.gameObject;
        skeletonAnimation = AnimationBody.GetComponent<SkeletonAnimation>();
    }

    private string currentAnim;

    public void AnimationLogic()
    {
        if (skeletonAnimation == null) return;

        string desiredAnim = (!isAttacking)
            ? (TravelManager.instance.IsTraveling ? "Walk" : "Idle")
            : "Attack";

        if (currentAnim != desiredAnim)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, desiredAnim, true);
            currentAnim = desiredAnim;
        }
    }

    private void HandleTravelStateChanged(bool traveling)
    {
        if (!isAttacking)
        {
            skeletonAnimation.state.SetAnimation(0, traveling ? "Walk" : "Idle", true).MixDuration = 0.2f;
            currentAnim = traveling ? "Walk" : "Idle";
        }
    }

    private void OnAnimationComplete(Spine.TrackEntry trackEntry)
    {
        // Only care about Attack animation finishing
        if (trackEntry.Animation.Name == "Attack")
        {
            isAttacking = false;
            AnimationLogic(); // return to Idle or Walk
        }
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
        if (targetPoint == null) return;

        EnemyUnit enemytarget = other.GetComponent<EnemyUnit>();
        if (attackCooldown <= 0f && enemytarget != null)
        {
            isAttacking = true;
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
            skeletonAnimation.AnimationState.AddAnimation(0,TravelManager.instance.IsTraveling ? "Walk" : "Idle",true,0f);

            Attack(enemytarget);
            Knockback(enemytarget);
            attackCooldown = 1f / runtimeData.attackSpeed;
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
            targetrb.AddForce(Vector2.up * knockbackForce / 2, ForceMode2D.Impulse); // slight upward force
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
        Inventory.instance.DeleteItem(inventoryIcon.gameObject);
        
        catGO.SetActive(false);
        //Destroy(gameObject);  //delete when return to lobby.
    }

    public void AssignCat(CatRuntimeData runtimeCat)
    {
        runtimeData = runtimeCat;

        // If using SpriteRenderer (simple 2D icon)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = runtimeCat.template.icon;
        }
        else
        {
            Debug.LogWarning($"{name} has no SpriteRenderer, skipping icon assignment.");
        }

        // If using Spine SkeletonGraphic (UI)
        SkeletonGraphic sg = GetComponent<SkeletonGraphic>();
        if (sg != null)
        {
            string skinName = runtimeCat.template.skinName;
            Skin skin = sg.Skeleton.Data.FindSkin(skinName);
            if (skin != null)
            {
                sg.Skeleton.SetSkin(skin);
                sg.Skeleton.SetSlotsToSetupPose();
                sg.AnimationState.Apply(sg.Skeleton);
            }
            else
            {
                Debug.LogWarning($"Skin '{skinName}' not found for {name}");
            }
        }

        // If using Spine SkeletonAnimation (world object)
        SkeletonAnimation sa = GetComponent<SkeletonAnimation>();
        if (sa != null)
        {
            string skinName = runtimeCat.template.skinName;
            Skin skin = sa.Skeleton.Data.FindSkin(skinName);
            if (skin != null)
            {
                sa.Skeleton.SetSkin(skin);
                sa.Skeleton.SetSlotsToSetupPose();
                sa.AnimationState.Apply(sa.Skeleton);
            }
            else
            {
                Debug.LogWarning($"Skin '{skinName}' not found for {name}");
            }
        }
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
