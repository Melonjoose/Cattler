using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public CapsuleCollider2D triggerCollider2D;
    public ItemRuntimeData runtimeData;

    public CatUnit catUnit; //cat that is equipping this item.

    private void Awake()
    {
        //if this gameobject has collider2D, get Collider2D and add it to collider2D
        triggerCollider2D = GetComponent<CapsuleCollider2D>();

    }
    private void Start()
    {
        runtimeData = new ItemRuntimeData(runtimeData.template);

        // Only randomize when runtimeData exists
        if (runtimeData != null)
            RandomizeStats();
    }

    protected virtual void RandomizeStats()
    {
        // Generate a unique random multiplier for each stat
        float rollHealth = Random.Range(0.6f, 1.5f);
        float rollAttackPower = Random.Range(0.6f, 1.5f);
        float rollAttackSpeed = Random.Range(0.6f, 1.5f);
        float rollAttackRange = Random.Range(0.6f, 1.5f);
        float rollMovementSpeed = Random.Range(0.6f, 1.5f);

        runtimeData.health = Mathf.RoundToInt(runtimeData.template.health * rollHealth);
        runtimeData.attackPower = Mathf.RoundToInt(runtimeData.template.attackPower * rollAttackPower);
        runtimeData.attackSpeed = Mathf.RoundToInt(runtimeData.template.attackSpeed * rollAttackSpeed);
        runtimeData.attackRange = Mathf.RoundToInt(runtimeData.template.attackRange * rollAttackRange);
        runtimeData.movementSpeed = Mathf.RoundToInt(runtimeData.template.movementSpeed * rollMovementSpeed);

        
        Debug.Log($"{name} randomized stats:\n" +
            $"Health {rollHealth * 100:F0}% | " +
            $"Atk {rollAttackPower * 100:F0}% | " +
            $"AS {rollAttackSpeed * 100:F0}% | " +
            $"Range {rollAttackRange * 100:F0}% | " +
            $"Move {rollMovementSpeed * 100:F0}%");
    
    }

    private bool hasCommented = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Collider2D>() != null)
        {
            if (collision.gameObject.CompareTag("Cat"))
            {
                if(Inventory.instance.isFull != true)
                {
                    AddItemtoInventory(this);
                }
                else if(!hasCommented)
                {
                    CommentaryManager.instance.AddDialogueToQueue(4); // Inventory full dialogue
                    hasCommented = true;
                }
            }
        }
    }

    void AddItemtoInventory(Item item)
    {
        
        Inventory.instance.InstantiateNewItem(item);
        CollectItem();
        Currency.instance.itemsEarnedThisMission++; //plus 1 item int to itemsEarnedThisMission in Currency script, for calculating score at the end of the mission. 
    }

    void CollectItem()
    {
        AudioManager.instance.PlaySFX("Collect");
        Destroy(gameObject);
        //later can be a sequence.
    }

}

