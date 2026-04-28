using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public StaticCoat staticCoat;
    public SkeletonAnimation skeletonAnimation;
    public Collider2D lightningCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //staticCoat = this.GetComponentInParent<StaticCoat>(); //Its not inside staticcoat.
        skeletonAnimation = this.GetComponentInChildren<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, "animation", false);
        lightningCollider = this.GetComponent<Collider2D>();
        lightningCollider.enabled = false;
        skeletonAnimation.state.Complete += HandleStageComplete;
        StartCoroutine(LightningSequence());
    }

    private void HandleStageComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "animation")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator LightningSequence()
    {
        yield return new WaitForSeconds(0.4f);
        lightningCollider.enabled = true;
    }

    private HashSet<EnemyUnit> hitEnemies = new HashSet<EnemyUnit>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemyUnit = collision.GetComponent<EnemyUnit>();
        if (enemyUnit != null && !hitEnemies.Contains(enemyUnit))
        {
            hitEnemies.Add(enemyUnit);
            enemyUnit.TakeDamage(staticCoat.catUnit, 3, 3);
            DebuffManager.instance.ApplyDebuff(enemyUnit.gameObject, DebuffManager.instance.stun, 2f);
        }
    }

}
