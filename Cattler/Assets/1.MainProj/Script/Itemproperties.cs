using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    public ConsumableData consumableData;
    private CapsuleCollider2D col;
    private CatUnit cat;
    private SpriteRenderer spriteRenderer;

    void Start()
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

    void Consume()
    {
        if (consumableData != null)
        {
            PotionChecker();

            SFX popup = SFXManager.instance.GetFromPool();
            popup.transform.position = this.transform.position;
            popup.SetText(consumableData.value , consumableData.itemNameShort);
            popup.SetIcon(spriteRenderer.sprite);

        }
        Destroy(gameObject); // Destroy the item object fully
    }

    void PotionChecker()
    {
        if (consumableData.itemName == "MaxHealthPotion")
        {
            MaxHealthPotion(consumableData.value);
        }
        if(consumableData.itemName == "AttackPotion")
        {
            AttackPotion(consumableData.value);
        }
    }

    void MaxHealthPotion(int valueAdded)
    {
        // Increase max health
        cat.runtimeData.maxHealth += valueAdded;

        // Heal cat but clamp so it doesn’t go past new max
        cat.runtimeData.currentHealth = Mathf.Min(
            cat.runtimeData.currentHealth + valueAdded,
            cat.runtimeData.maxHealth
        );
    }

    void AttackPotion(int valueAdded)
    {
        cat.runtimeData.attackPower += valueAdded;
       
    }

}
