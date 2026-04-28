using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedBall : MonoBehaviour
{
    public BouncingBall bouncingball;

    void Start()
    {
        StartCoroutine(BouncingBallSequence());
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * 360f * Time.deltaTime);
        // 360 degrees per second, adjust speed as needed
    }

    private HashSet<EnemyUnit> hitEnemies = new HashSet<EnemyUnit>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit unit = collision.GetComponent<EnemyUnit>();
        if (unit != null && !hitEnemies.Contains(unit))
        {
            hitEnemies.Add(unit);
            unit.TakeDamage(bouncingball.catUnit, 5, 1);
            StartCoroutine(FlattenEnemy(unit.transform));
            Debug.Log("Enemy hit by RedBall!");
        }

        // Check if collided with floor layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            // Play bounce audio
            AudioManager.instance.PlaySFX("Bounce");
            Debug.Log("RedBall bounced on floor!");
        }
    }

    IEnumerator BouncingBallSequence()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;  //static first
        yield return new WaitForSeconds(0.5f);

        // Grow from (1,1,1) to (6,6,6)
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(6, 6, 6), t);
            yield return null;
        }

        //while its running, rotate this object at constant speed

        // Launch to the right and slightly upward

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(new Vector2(4f, 10f), ForceMode2D.Impulse);
        }
    }

    private IEnumerator FlattenEnemy(Transform enemy)
    {
        // Save original scale
        Vector3 originalScale = enemy.localScale;
        Vector3 flattenedScale = new Vector3(originalScale.x, originalScale.y * 0.2f, originalScale.z);

        // Smoothly squash down
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 4f; // speed multiplier
            enemy.localScale = Vector3.Lerp(originalScale, flattenedScale, t);
            yield return null;
        }

        // Hold flattened for a short time
        yield return new WaitForSeconds(0.5f);

        // Smoothly restore
        t = 0f;
        while (t < 1f && enemy != null)
        {
            t += Time.deltaTime * 4f;
            enemy.localScale = Vector3.Lerp(flattenedScale, originalScale, t);
            yield return null;
        }

        // Ensure exact reset
        enemy.localScale = originalScale;
    }

}
