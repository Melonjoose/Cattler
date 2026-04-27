using UnityEngine;
using System.Collections;

public class WordProjectile : MonoBehaviour
{
    public DangerousSpeech dangerousSpeech;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform enemy = RandomEnemy();
        if (enemy != null)
        {
            StartCoroutine(RotateAndLaunch(enemy));
        }
        
    }

    // lock onto enemy when this is spawned. rotate towards enemy.
    // once locked on, fire this object towards enemy.
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit unit = collision.GetComponent<EnemyUnit>();
        if (unit != null)
        {
            unit.TakeDamage(dangerousSpeech.catUnit, 4, 2);
            Destroy(this.gameObject);
        }
    }

    IEnumerator RotateAndLaunch(Transform enemy)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // If enemy is gone, find a new one
            if (enemy == null)
            {
                enemy = RandomEnemy();
                if (enemy == null)
                {
                    // No enemies left > destroy projectile
                    Destroy(gameObject);
                    yield break;
                }
            }

            // Recalculate direction each frame
            Vector3 dir = (enemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / duration);

            yield return null;
        }

        // Snap to final rotation
        if (enemy != null)
        {
            Vector3 dir = (enemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            yield return new WaitForSeconds(0.2f);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 0;

                // Use latest direction at launch
                rb.AddForce((enemy.position - transform.position).normalized * 18f, ForceMode2D.Impulse);
            }
        }
        else
        {
            // No enemy at launch > destroy projectile
            Destroy(gameObject);
        }
    }


    public Transform RandomEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        int randomIndex = Random.Range(0, enemies.Length);
        return enemies[randomIndex].transform;
    }
}
