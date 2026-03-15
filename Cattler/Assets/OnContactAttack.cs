using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnContactAttack : MonoBehaviour
{
    public CatUnit catUnit;
    public Skill_Sweep skill;
    public SkeletonAnimation skeleton;
    public Collider2D col;
    public Rigidbody2D body;
    public int damage;
    void Start()
    {
        col = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        // Subscribe to animation complete event
        skeleton.AnimationState.Complete += OnAnimationComplete;

    }

    public HashSet<EnemyUnit> enemiesHit = new HashSet<EnemyUnit>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemy = collision.GetComponent<EnemyUnit>();
        if (enemy == null) return;

        if (enemiesHit.Contains(enemy)) return; // already hit this enemy

        enemiesHit.Add(enemy);

        int damage = (catUnit != null) ? catUnit.runtimeData.attackPower : 4;
        enemy.TakeDamage(damage);
        Knockback(enemy);
        DamageNumberManager.Instance.ShowDamage(damage, collision.transform.position);

        Debug.Log($"Hit enemy {enemy.name} once");
    }

    private void OnAnimationComplete(Spine.TrackEntry trackEntry)
    {
        // Clear hit list when the animation finishes
        enemiesHit.Clear();
        Debug.Log("Attack animation complete, hit list reset.");
    }
    public void ClearHitList()
    {
        enemiesHit.Clear();
        Debug.Log("Hit list cleared");
    }

    private void Knockback(EnemyUnit target)
    {
        Rigidbody2D targetrb = target.GetComponent<Rigidbody2D>();
        if (targetrb != null)
        {
            Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;
            float knockbackForce = 5f; // Adjust force as needed
            targetrb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            targetrb.AddForce(Vector2.up * knockbackForce / 2, ForceMode2D.Impulse); // slight upward force
        }
    }

}
