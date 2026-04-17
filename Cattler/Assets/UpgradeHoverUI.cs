using Spine.Unity;
using UnityEngine.EventSystems;

public class UpgradeHoverUI : HoverOnUI
{
    public SkeletonGraphic skeletonAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonGraphic>();
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Hovering_Transition", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Hovering_Idle", true, 0f);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        }
    }
}
