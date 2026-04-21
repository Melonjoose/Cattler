using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Canvas debuffCanvas; //holds the debuff icons, assign in inspector

    public bool canAttack = true; //can this unit attack?
    public bool canTarget = true; //can this unit be targetted?
    public bool isAttacking = false; //is this unit in the midst of attacking?
    public bool canWalk = true; //can this unit walk?
    public bool isSlowed = false; //is this unit slowed?
    public bool isDead = false; //is this unit dead?
    public SkeletonAnimation skeletonAnimation;
    public SkeletonGraphic skeletonGraphic;


    public bool isStunned;

    [SerializeField]private List<BuffInstance> activeBuffs = new List<BuffInstance>(); //all the debuff currently applied onto this unit. //to be addedlater.
    [SerializeField]private List<DebuffInstance> activeDebuffs = new List<DebuffInstance>(); //all the debuff currently applied onto this unit.

    private void Awake()
    {
        Transform child = transform.Find("DebuffCanvas");
        if (child != null)
        {
            debuffCanvas = child.GetComponent<Canvas>();
            Debug.Log("DebuffCanvas found and linked on " + gameObject.name);
        }
        else
        {
            Debug.LogWarning("DebuffCanvas not found on " + gameObject.name);
        }
    }
    private void Start()
    {
        OnUnitStart();

    }

    protected virtual void OnUnitStart()
    {
        // Default behavior (optional)
    }


    void Update() // sealed: subclasses cannot override
    {
        TickDebuffs(Time.deltaTime);
        TickBuffs(Time.deltaTime);
        ForceDeath(5f);
        OnUnitUpdate(); // hook for subclasses
    }
    protected virtual void OnUnitUpdate() { }

    private float deathCheckerTimer = 0f; // persistent field

    void ForceDeath(float interval)
    {
        // accumulate time
        deathCheckerTimer += Time.deltaTime;
        if (deathCheckerTimer >= interval)
        {
            deathCheckerTimer = 0f; // reset timer

            if (isDead)
            {
                Debug.Log($"Safeguard: Destroying {gameObject.name} because it is flagged dead.");
                Destroy(gameObject); // correct Unity API call
            }
        }
    }


    private void TickDebuffs(float deltaTime)
    {
        // Iterate backwards so we can safely remove expired debuffs
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            DebuffInstance instance = activeDebuffs[i];

            // Tick down the timer
            bool expired = instance.Tick(deltaTime);

            // If less than 1 second left, trigger blinking UI
            if (instance.remainingTime <= 1.5f)
            {
                instance.BlinkingUI();
            }

            if (expired)
            {
                // Remove effects + UI
                instance.RemoveDebuffEffect();
                instance.RemoveUI();

                // Remove from list
                activeDebuffs.RemoveAt(i);
            }
        }
    }

    private void TickBuffs(float deltaTime)
    {
        // Iterate backwards so we can safely remove expired debuffs
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            BuffInstance instance = activeBuffs[i];

            // Tick down the timer
            bool expired = instance.Tick(deltaTime);
            // Apply health regen if this buff is a regen type
            instance.HealthRegen(deltaTime);
            // If less than 1 second left, trigger blinking UI
            if (instance.remainingTime <= 1.5f)
            {
                instance.BlinkingUI();
            }

            if (expired)
            {
                // Remove effects + UI
                instance.RemoveBuffEffect();
                instance.RemoveUI();

                // Remove from list
                activeBuffs.RemoveAt(i);
            }
        }
    }

    public void AddDebuff(Debuff debuff, float duration)
    {
        var instance = new DebuffInstance(debuff, this, duration);
        activeDebuffs.Add(instance);

        instance.ApplyDebuffEffect();
    }


    public virtual void OnDebuffRemoved(Debuff debuff)
    {
        var existing = activeDebuffs.Find(d => d.debuff == debuff);
        if (existing != null)
        {
            activeDebuffs.Remove(existing);
            existing.RemoveDebuffEffect();
        }
    }

    public void RemoveAllDebuffs() //clear all debuff from this unit.
    {
        foreach (var instance in activeDebuffs)
        {
            instance.RemoveDebuffEffect();
            instance.RemoveUI();
        }
        activeDebuffs.Clear();
    }

    public void AddBuff(Buff buff, float duration , float strength)
    {
        var instance = new BuffInstance(buff, this, duration);
        activeBuffs.Add(instance);

        instance.ApplyBuffEffect();
        instance.ShowUI();
    }




    public virtual void TakeDamage(Unit hitter, int damage , float knockback)
    {
        // Default implementation (can be overridden) //base will call knockback and damage number.
        Debug.Log($"{gameObject.name} took {damage} damage.");
        if (knockback > 0f)
        {
            Knockback(hitter, knockback);
        }
        DamageNumberManager.Instance.ShowDamage(damage, this.gameObject.transform.position); //Showdamage at location (SHOWS DMG TAKEN)
        //VFX can be added here later.
    }
    public void Knockback(Unit hitter,float knockback)
    {
        Rigidbody2D targetrb = this.GetComponent<Rigidbody2D>();
        if (targetrb != null)
        {
            if(hitter == null)
            {
                Vector2 knockbackDirectionDefault = Vector2.right;  //default to push towards the right if hitter is null
                targetrb.AddForce(knockbackDirectionDefault * knockback, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 knockbackDirection = (this.transform.position - hitter.transform.position).normalized;
                targetrb.AddForce(knockbackDirection * knockback, ForceMode2D.Impulse);
                targetrb.AddForce(Vector2.up * knockback / 2, ForceMode2D.Impulse); // slight upward force
            }
        }
    }
}