using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    public ConsumableData consumableData;
    private CapsuleCollider2D col;
    private CatUnit cat;

    void Start()
    {
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
        Debug.Log("consume runned");
        if (consumableData != null)
        {
            PotionChecker();
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
