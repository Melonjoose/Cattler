using UnityEngine;

public class IceCream : MonoBehaviour
{
    public FrostyTreat frostyTreat;
    public Collider2D iceCreamCollider;
    public Puddle puddle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(true);
        iceCreamCollider = GetComponent<Collider2D>();
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
            iceCreamCollider.enabled = false;
            puddle.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX("Splat"); // Play hit sound effect
            IcecreamExpriry();
        }

    }

    public void IcecreamExpriry()
    {
        //after 5 seconds, destroy this object
        Destroy(gameObject, 5f);
    }
    //upon contact with the floor, ice cream will set the puddle.gameobject.setactive. a puddle that will slow down enemies that walk 
}
