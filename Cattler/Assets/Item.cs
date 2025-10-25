using UnityEngine;

public class Item : MonoBehaviour
{
    public Collider2D collider2D;
    public ItemRuntimeData runtimeData;

    public CatUnit catUnit; //cat that is equipping this item.

    private void Awake()
    {
        //if this gameobject has collider2D, get Collider2D and add it to collider2D
        collider2D = GetComponent<Collider2D>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collider2D != null)
        {
            if (collision.gameObject.CompareTag("Cat"))
            {
                AddItemtoInventory();
            }
        }
    }

    void AddItemtoInventory()
    {
        Inventory.instance.InstantiateNewWeapon(this);
        Destroy(gameObject);
    }
}

