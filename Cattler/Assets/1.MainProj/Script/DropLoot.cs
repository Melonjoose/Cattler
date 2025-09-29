using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [System.Serializable]
    public class LootTableEntry
    {
        public GameObject item;
        public float dropChance; // Probability weight
    }
    public LootTableEntry[] lootTable; // Assign in inspector
    public float chanceToDropNothing = 0.5f; // 50% chance to drop nothing

    public int InkDrop = 10;
    public int CoreDrop = 0;
    public int EXPDrop = 10;

    private EnemyUnit enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<EnemyUnit>();
    }

    public GameObject Roll()
    {
        float totalWeight = 0f;
        // Include the "nothing" chance in total weight
        foreach (var entry in lootTable)
            totalWeight += entry.dropChance;
        totalWeight += chanceToDropNothing;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        // Check if roll falls into the "nothing" range first
        cumulative += chanceToDropNothing;
        if (roll <= cumulative)
            return null;

        // Otherwise, iterate loot table
        foreach (var entry in lootTable)
        {
            cumulative += entry.dropChance;
            if (roll <= cumulative)
                return entry.item;
        }

        return null; // fallback
    }

    public void GiveLoot()
    {
        GameObject dropPrefab = Roll();
        if (dropPrefab != null)
        {
            GameObject dropInstance = Instantiate(dropPrefab, enemy.transform.position, Quaternion.identity);
        }

    }
}