using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CatSkill : MonoBehaviour
{
    public bool isActive; // whether the skill is active or passive
    public string skillDesc;
    public string skillType;
    public float cooldown; // cooldown time for the skill, if applicable
    public float time; //time for cooldown or duration of the skill, depending on the skill type
    public float value; // this can be used for different purposes depending on the skill, such as damage multiplier, heal amount, etc.
    public float speed; // this can be used for skills that affect movement speed or attack speed, etc.

    public GameObject itemObject;
    public CatUnit catUnit; // this is attached to this catUnit.

    protected virtual void Start()
    {
        catUnit = GetComponentInParent<CatUnit>();
        RandomizeTimerStart(); // Randomize the starting point of the timer to create more dynamic skill usage
    }

    void FixedUpdate() // Use FixedUpdate for consistent timing, especially if the skill's effects are time-sensitive
    {
        Cooldown();
    }
    public void Cooldown()
    {
        if (isActive == false)
        {
            return;
        }
        if (time <= 0)//check if the cooldown time has elapsed
        {
            UseSkill();
            time = cooldown; //reset time to match the cooldown duration
        }

        time -= Time.deltaTime; //decrease time by the time that has passed since the last frame
    }
    public virtual void UseSkill()
    {
        if(catUnit == null)
        {
            catUnit = GetComponentInParent<CatUnit>();
        }   
        if (catUnit != null) // animation   
        {
            catUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Skill1", false);
            catUnit.skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true , 0f);
        }
    }


    public void RandomizeTimerStart()
    {
        //from 0 to cooldown, randomize the starting point of the timer to create more dynamic skill usage
        time = Random.Range(0, cooldown);
    }
}
