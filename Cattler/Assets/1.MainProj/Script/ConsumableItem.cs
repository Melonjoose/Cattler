using System;
using UnityEngine;

public class ConsumableItem : MonoBehaviour
{
    public ConsumableData consumableData;
    private CapsuleCollider2D col;
    public CatUnit cat;
    private SpriteRenderer spriteRenderer;
    public event Action onConsumed;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Grab the CatUnit component from the other object
        cat = collision.gameObject.GetComponent<CatUnit>();
        if (cat != null)
        {
            Consume();
        }
    }

    protected virtual void Consume()
    {
        if (consumableData != null)
        {
            onConsumed?.Invoke(); //send to listener
        }
        Destroy(gameObject); // Destroy the item object fully
    }

}
