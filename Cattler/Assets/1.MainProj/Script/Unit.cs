using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Canvas debuffCanvas; //holds the debuff icons, assign in inspector

    public bool isAttacking = false;
    public bool canWalk = true;
    public bool isDead = false;
    public SkeletonAnimation skeletonAnimation;
    public SkeletonGraphic skeletonGraphic;

    public bool canAttack = true;

    public bool isStunned;

    [SerializeField]private List<DebuffInstance> activeBuffs = new List<DebuffInstance>(); //all the debuff currently applied onto this unit. //to be addedlater.
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
        Debug.Log($"Unit {gameObject.name} has {activeDebuffs.Count} active debuffs.");
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

    public void AddDebuff(Debuff debuff)
    {
        var instance = new DebuffInstance(debuff, this);
        activeDebuffs.Add(instance);

        instance.ApplyDebuffEffect();
        instance.ShowUI();
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

    public void RemoveAllDebuffs()
    {
        foreach (var instance in activeDebuffs)
        {
            instance.RemoveDebuffEffect();
            instance.RemoveUI();
        }
        activeDebuffs.Clear();
    }

}