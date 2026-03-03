using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [System.Serializable]
    public class LootTableEntry
    {
        public GameObject item;
        [Range(0f, 100f)]  // 0 % to 100%
        public float dropChance; // Percentage chance to drop this item
    }
    public LootTableEntry[] lootTable; // Assign in inspector

    public int minInkDrop = 10;
    public int maXInkDrop = 100;
    public int minCoreDrop = 0;
    public int maxCoreDrop = 0;
    public int minEXPDrop = 10;
    public int maxEXPDrop = 20;

    private EnemyUnit enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<EnemyUnit>();
    }

    public GameObject Roll()
    {
        float roll = Random.Range(0f, 100f); // roll between 0 and 100
        float cumulative = 0f;

        // Iterate loot table
        foreach (var entry in lootTable)
        {
            cumulative += entry.dropChance;
            if (roll <= cumulative)
                return entry.item;
            //Debug.Log("Rolled " + roll + " needed less than " + cumulative + " for " + entry.item.name);
        }

        return null; // fallback
    }


    public int RollInk()
    {
        return Random.Range(minInkDrop, maXInkDrop);
    }

    public int GetEXP()
    {
        return Random.Range(minEXPDrop , maxEXPDrop);
    }

    public int GetCoreDrop()
    {
        return Random.Range(minCoreDrop, maxCoreDrop);
    }

    public void GiveLoot()
    {
        GameObject dropPrefab = Roll();
        if (dropPrefab != null)
        {
            GameObject dropInstance = Instantiate(dropPrefab, enemy.transform.position, Quaternion.identity);
            StatFXManager.instance.PlayVFX(dropInstance.transform.position, 3); // drop vfx

        }

        Currency.instance.AddInk(RollInk());
        Currency.instance.AddCore(GetCoreDrop());
        Currency.instance.AddEXP(GetEXP());
    }
}