using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Grandstage : CatSkill

{
    public SkeletonAnimation stageAnimator;
    public SkeletonAnimation catAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.RandomizeTimerStart();
        catUnit = GetComponentInParent<CatUnit>();
        catAnimator = catUnit.skeletonAnimation;

        stageAnimator = itemObject.GetComponent<SkeletonAnimation>();
        if (stageAnimator != null)
        {
            stageAnimator.state.Complete += HandleStageComplete;
            stageAnimator.AnimationState.SetAnimation(0, "Stage_Hidden", true);
        }
    }


    private void HandleStageComplete(Spine.TrackEntry trackEntry)
    {

        if (trackEntry.Animation.Name == "Stage_Enter")
        {
            stageAnimator.AnimationState.SetAnimation(0, "Stage_Hidden", true);
            EndSkillState();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkill()
    {
        Debug.Log("GrandStage skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        yield return new WaitForSeconds(0.5f);
        itemObject.SetActive(true);
        AudioManager.instance.PlaySFX("IdolCatMusic");
        AudioManager.instance.PlaySFX("Twinkle");

        if(stageAnimator != null)
        {
            stageAnimator.AnimationState.SetAnimation(0, "Stage_Enter", false);
            //upon completion. EndSkillState();
        }

        if(catAnimator != null)
        {
            catAnimator.AnimationState.SetAnimation(0, "StageSkill", false);
            SkillState();
        }

        yield return new WaitForSeconds(0.5f);
        ApplyBuffsToAllCats();
    }


    void SkillState() //0.
    {
        Healthbar healthbar = catUnit.GetComponent<Healthbar>();
        if (healthbar != null)
        {
            healthbar.healthbarLocationOffset += new Vector3(0, 2, 0);
        }
        isActive = false; //stop timer
        catUnit.canAttack = false; //cat cannot attack during the skill
        catUnit.canWalk = false; //cat cannot move during the skill
        catUnit.canTarget = false; //cat cannot be targeted by the enemyunits during the skill
        catUnit.inAnimation = true;

    }

    void EndSkillState() //7.
    {
        Healthbar healthbar = catUnit.GetComponent<Healthbar>();
        if (healthbar != null)
        {
            healthbar.healthbarLocationOffset += new Vector3(0, -2, 0);
        }
        isActive = true; //start timer again
        catUnit.canAttack = true; //cat can attack again after the skill
        catUnit.canWalk = true; //cat can move again after the skill
        catUnit.canTarget = true; //cat can be targeted by the enemyunits again after the skill
        catUnit.inAnimation = false;
        if (TravelManager.instance.IsTraveling) //if it's currently traveling, change animation to travel animation. if not, change to idle animation
        { catUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true); }//1. change animation to travel
        else
        { catUnit.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true); } //1. change animation to idle
    }

    void ApplyBuffsToAllCats()
    {
        foreach(CatUnit cat in TeamManager.instance.cats)
        {
            BuffManager.instance.ApplyBuff(cat.gameObject, BuffManager.instance.attackSpeedUp, 8f, 20f);
            BuffManager.instance.ApplyBuff(cat.gameObject, BuffManager.instance.range, 8f, 20f);
            BuffManager.instance.ApplyBuff(cat.gameObject, BuffManager.instance.damageUp, 8f, 20f);
        }
    }
}
