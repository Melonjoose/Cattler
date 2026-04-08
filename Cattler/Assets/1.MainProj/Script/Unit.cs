using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    public bool isAttacking = false;
    public bool canWalk = true;
    public bool isDead = false;

    public bool canAttack = true;

    public bool isStunned;

    [SerializeField]private List<DebuffInstance> activeBuffs = new List<DebuffInstance>(); //all the debuff currently applied onto this unit. //to be addedlater.
    [SerializeField]private List<DebuffInstance> activeDebuffs = new List<DebuffInstance>(); //all the debuff currently applied onto this unit.

    void Update() // sealed: subclasses cannot override
    {
        TickDebuffs();
        Debug.Log($"Unit {gameObject.name} has {activeDebuffs.Count} active debuffs.");
        OnUnitUpdate(); // hook for subclasses
    }
    protected virtual void OnUnitUpdate() { }


    protected virtual void TickDebuffs()
    {
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            if (activeDebuffs[i].Tick(Time.deltaTime))
            {
                OnDebuffRemoved(activeDebuffs[i].debuff);
                activeDebuffs.RemoveAt(i);
            }
        }
    }

    public virtual void OnDebuffApplied(Debuff debuff)
    {
        var existing = activeDebuffs.Find(d => d.debuff == debuff);
        if (existing != null)
        {
            // Refresh duration instead of duplicating
            existing.remainingTime = debuff.duration;
        }
        else
        {
            activeDebuffs.Add(new DebuffInstance(debuff));
        }

        if (debuff.debuffType == Debuff.DebuffType.Stun)
            ApplyStun();
    }

    public virtual void OnDebuffRemoved(Debuff debuff)
    {
        if (debuff.debuffType == Debuff.DebuffType.Stun)
            Unstun();
        //else if (debuff.debuffType == Debuff.DebuffType.Slow)
            //RemoveSlow();
    }

    private void ApplyStun()
    {
        isStunned = true;
        canAttack = false;
        canWalk = false;
        // disable movement/attack here
    }

    public void Unstun()
    {
        isStunned = false;
        canAttack = true;
        canWalk = true;
        // restore movement/attack here
    }

    //protected abstract void ApplySlow(float amount);
    //protected abstract void RemoveSlow();
    //protected abstract void ApplyPoison(float dps);
}