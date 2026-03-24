using Spine.Unity;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_Sweep : CatSkill
{
    public SkeletonAnimation skeleton;
    public Rigidbody2D rb;
    public GameObject colPoint; //holds col
    public GameObject colBody;
    public OnContactAttack contactAttack;
    public Collider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skeleton = GetComponent<SkeletonAnimation>();
        colPoint = transform.Find("ColliderPoint")?.gameObject;
        colBody = colPoint.transform.Find("Collider")?.gameObject;
        col = colBody.GetComponent<Collider2D>();
        rb = colBody.GetComponent<Rigidbody2D>();
        //catUnit = this.GetComponentfromparentgameobject<CatUnit>();    skill_sweep is a child of catUnit.

        contactAttack = colBody.GetComponent<OnContactAttack>();
        SyncCollider();

        skeleton.AnimationState.Event += HandleSpineEvent;
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
    public void UseSkill()
    {
        // Implement the logic for Sweep skill here
        //turn on collider of object. ensure toggleCollider() is working properly.
        //start collider2D.offset and size to being small.
        //make the collider grow into the default size. offset (-1.004715,9.168252) size (2.509531,20.84606).
        //play animation.
        //turn off collider of object. ensure toggle collider is working correctly.
        skeleton.AnimationState.SetAnimation(0, "animation", false);
    }

    void SyncCollider()
    {
        contactAttack.skill = this;
        contactAttack.catUnit = catUnit;
        contactAttack.skeleton = skeleton;
        contactAttack.col = col;
    }

    private void HandleSpineEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "ToggleCollider") //handling toggle
        {
            if (e.Int == 1)
            {
                col.enabled = true;
                Debug.Log("Collider enabled");
            }
            else if (e.Int == 0)
            {
                col.enabled = false;
                Debug.Log("Collider disabled");

                // Clear hit list when collider turns off
                contactAttack.ClearHitList();

            }
        }

    }

}
