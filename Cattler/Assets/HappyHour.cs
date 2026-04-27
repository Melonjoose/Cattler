using UnityEngine;
using Spine.Unity;

public class HappyHour : CatSkill
{
    public SkeletonAnimation clockAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        clockAnim = GetComponentInChildren<SkeletonAnimation>();
        // Restart clock from beginning
        clockAnim.AnimationState.SetAnimation(0, "animation", true);
        if (clockAnim != null)
        {
            // Subscribe to Spine animation events
            clockAnim.state.Event += HandleClockEvent;
        }
    }

    private void HandleClockEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "HappyTrigger")
        {
            HappyTrigger();
        }
        else if (e.Data.Name == "SadTrigger")
        {
            SadTrigger();
        }
    }

    void HappyTrigger()
    {
        if (isActive)
        {
            // Change skin to OfficeCat(Night)
            var skeleton = catUnit.skeletonAnimation.Skeleton;
            skeleton.SetSkin("OfficeCat(Night)");
            skeleton.SetSlotsToSetupPose(); // refresh slots to apply skin
            catUnit.skeletonAnimation.AnimationState.Apply(skeleton);

            // Buffs
            BuffManager.instance.ApplyBuff(catUnit.gameObject, BuffManager.instance.attackSpeedUp, 4, 50);
            BuffManager.instance.ApplyBuff(catUnit.gameObject, BuffManager.instance.damageUp, 4, 50);
            BuffManager.instance.ApplyBuff(catUnit.gameObject, BuffManager.instance.range, 4, 20);

            Debug.Log("HappyHour buffs applied and skin changed to Night!");
        }
    }
    void SadTrigger()
    {
        if (isActive)
        {
            // Change skin back to OfficeCat(Day)
            var skeleton = catUnit.skeletonAnimation.Skeleton;
            skeleton.SetSkin("OfficeCat(Day)");
            skeleton.SetSlotsToSetupPose();
            catUnit.skeletonAnimation.AnimationState.Apply(skeleton);

            Debug.Log("SadTrigger fired — skin changed back to Day.");
        }
    }
}
