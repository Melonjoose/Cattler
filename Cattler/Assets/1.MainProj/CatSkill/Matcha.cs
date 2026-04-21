using UnityEngine;

public class Matcha : MonoBehaviour
{
    public SpilledDrink spilledDrink;
    public Collider2D collider2D;
    public Puddle puddle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(true);
        collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Activate the puddle
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            collider2D.enabled = false;
            puddle.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX("GlassBreak"); // Play hit sound effect
            Expiry();
        }

    }

    public void Expiry()
    {
        //after 5 seconds, destroy this object
        Destroy(gameObject, 5f);
    }
}
