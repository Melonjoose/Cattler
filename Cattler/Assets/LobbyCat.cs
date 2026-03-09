using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class LobbyCat : MonoBehaviour
{
    public CatUnit catUnit;
    public SkeletonGraphic skeletonAnimation;
    public string skinName;
    public string walkAnimationName = "Walk";
    public string idleAnimationName = "Idle";
    public float moveSpeed = 2f; // units per second

    void Start()
    {
        catUnit = GetComponent<CatUnit>();
        skeletonAnimation = GetComponent<SkeletonGraphic>();
        ChangeSkin();
    }
    public void ChangeSkin() 
    {
        LobbyCat catLobbyScript = this;
        if (catLobbyScript != null)
        {
            SkeletonGraphic skeletonGraphic = catLobbyScript.skeletonAnimation; //get the lobbycat.cs's skeletongraphic
            skinName = catUnit.runtimeData.template.skinName;
            if (skinName != null)
            {
                Skin skin = skeletonGraphic.Skeleton.Data.FindSkin(skinName);
                if (skin != null)
                {
                    skeletonGraphic.Skeleton.SetSkin(skin);
                    skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                    skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);
                }
                else
                {
                    Debug.LogWarning($"Skin '{skinName}' not found in skeleton data.");
                }
            }
        }
    }

    void OnEnable()
    {
        StartCoroutine(RoamSequence());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RoamSequence()
    {
        
        while (true)
        {
            // Phase 1: Idle for random duration
            skeletonAnimation.AnimationState.SetAnimation(0, idleAnimationName, true);
            float idleTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(idleTime);

            // Phase 2: Pick a random target location
            Vector3 target = CatRoamLobby.instance.GetRandomPositionWithinBounds();

            // Phase 3: Walk smoothly to target
            skeletonAnimation.AnimationState.SetAnimation(0, walkAnimationName, true);
            yield return StartCoroutine(MoveTo(target));

            // Loop back to idle phase
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            // Flip based on direction
            Vector3 scale = transform.localScale;
            if (transform.position.x < target.x)
            {
                // Facing right
                scale.x = Mathf.Abs(scale.x);
            }
            else
            {
                // Facing left
                scale.x = -Mathf.Abs(scale.x);
            }
            transform.localScale = scale;

            yield return null; // wait one frame
        }
    }
}