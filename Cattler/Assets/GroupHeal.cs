using System.Collections;
using UnityEngine;

public class GroupHeal : ActiveAbility
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GroupHealSequence()); // Start the group heal sequence when the skill is used    
    }

    IEnumerator GroupHealSequence()
    {
        //play animation or particle effect for the group heal here (optional)
        yield return new WaitForSeconds(0.5f); // wait for the animation to play (adjust the time as needed)
        HealAllies(); // apply the healing effect to all allies after the animation is done
        yield return new WaitForSeconds(4.5f); // wait for the animation to play (adjust the time as needed)
        Destroy(gameObject); // destroy the skill object after the effect is applied and animation is done
    }

    //Apply healthregen to all allies in team list
    public void HealAllies()
    {
        foreach (CatUnit ally in TeamManager.instance.cats)
        {
            BuffManager.instance.ApplyBuff(ally.gameObject, BuffManager.instance.healthRegen, 5f, 3f); //apply health regen buff for 5 seconds with strength of 10 (healing 10 health per second)     
        }
    }
}
