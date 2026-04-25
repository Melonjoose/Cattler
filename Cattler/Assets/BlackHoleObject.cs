using Spine;
using Spine.Unity;
using UnityEngine;

public class BlackHoleObject : MonoBehaviour
{
    public SkeletonAnimation spineAnimation;
    public float pullRadius = 5f;
    public float pullStrength = 10f;
    public float damagePerSecond = 1f;
    public float duration = 5f;
    private float elapsedTime = 0f;
    private float damageTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //when the black hole is instantiated, play the entry animation
        spineAnimation.state.Event += HandleSpineEvent;
        spineAnimation.AnimationState.SetAnimation(0, "BlackHole_Entry", false);

    }

    private void HandleSpineEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "BlackHoleStart")
        {
            EnableVortexEffect();
        }

        if (e.Data.Name == "LoopTrigger")
        {
            spineAnimation.AnimationState.SetAnimation(0, "BlackHole_Loop", true);
        }

        if(e.Data.Name == "BlackHoleEnd")
        {
            Destroy(gameObject); // Destroy the black hole object when the exit animation finishes
        }
    }

    void EnableVortexEffect()
    {
        elapsedTime = 0f; // reset timer
        Debug.Log("Black hole vortex effect enabled!");
    }

    private bool EndTriggered = false;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        damageTimer += Time.deltaTime;

        if (elapsedTime >= duration && !EndTriggered)
        {
            EndVortexEffect();
            EndTriggered = true;
            return;
        }

        // Only apply damage once per second
        bool shouldDamage = false;
        if (damageTimer >= 1f)
        {
            shouldDamage = true;
            damageTimer = 0f; // reset
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pullRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                PullEnemy(col.transform);

                if (shouldDamage)
                {
                    DamageEnemy(col.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if the other object is an enemy
        if (other.CompareTag("Enemy"))
        {
            //pull the enemy towards the center of the black hole
            //apply damage over time to the enemy
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the pull radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }

    void PullEnemy(Transform enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = ((Vector2)transform.position - rb.position).normalized;
            rb.AddForce(direction * pullStrength);
        }
    }
    void DamageEnemy(GameObject enemy)
    {
        EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyUnit.TakeDamage(null, 1, 0);
        }
    }

    void EndVortexEffect()
    {
        spineAnimation.AnimationState.SetAnimation(0, "BlackHole_Exit", false);
        Debug.Log("Black hole vortex effect ended!");
    }
}
