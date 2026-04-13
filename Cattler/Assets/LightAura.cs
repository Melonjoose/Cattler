using System.Collections;
using UnityEngine;

public class LightAura : MonoBehaviour
{
    public BlindingLight blindingLight; // Reference to the BlindingLight skill that created this aura
    public float expansionSpeed = 1f; // Speed at which the light aura expands
    public float maxScale = 6f; // Maximum scale of the light aura
    public float lightlingerDuration = 0.5f; // Duration for which the light aura lingers after reaching max scale
    void Start()
    {
        blindingLight = GetComponentInParent<BlindingLight>(); // Get the BlindingLight component from the parent object
    }

    public void OnEnable()
    {
        //when this gameobject is enabled, intialize the data from blinding light
        //run the timer for expansion and lingering
            expansionSpeed = blindingLight.speed;
            lightlingerDuration = blindingLight.value;
            maxScale = 6f; // Set the maximum scale for the light aura
                           //reset the scale to zero when enabled
                           //reset opacity to 1 when enabled
        StartCoroutine(ExpandAndLinger());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ExpandAndLinger()
    {
        float elapsed = 0f;
        float expansionDuration = maxScale / expansionSpeed; // how long expansion should take
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        // Expansion phase with smooth slowdown
        while (elapsed < expansionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / expansionDuration);

            // SmoothStep gives a nice ease-in/ease-out curve
            float currentScale = Mathf.SmoothStep(0f, maxScale, t);
            transform.localScale = Vector3.one * currentScale;

            // Opacity goes from 0 > 0.8 as scale increases
            if (sr != null)
            {
                float alpha = Mathf.Lerp(0f, 0.8f, t);
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, alpha);
            }

            yield return null;
        }

        // Linger phase
        yield return new WaitForSeconds(lightlingerDuration);

        // Fade-out phase
        if (sr != null)
        {
            float fadeDuration = 0.5f;
            float fadeElapsed = 0f;
            Color originalColor = sr.color;

            while (fadeElapsed < fadeDuration)
            {
                fadeElapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(originalColor.a, 0f, fadeElapsed / fadeDuration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        // Deactivate for pooling
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is an enemy (you can use tags or layers to identify enemies)
        if (collision.CompareTag("Enemy"))
        {
            // Implement logic to apply stun effect to the enemy
            // For example, you can call a method on the enemy's script to apply the stun
            EnemyUnit enemy = collision.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                DebuffManager.instance.ApplyDebuff(enemy.gameObject, DebuffManager.instance.stun); // Apply a stun debuff for 2 seconds
            }
        }
    }
}
