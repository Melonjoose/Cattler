using UnityEngine;

public class SummonManager : MonoBehaviour

{
    public static SummonManager instance;

    [System.Serializable]
    public class GachaPoolEntry
    {
        public CatData catData;
        public float weight; // probability weight
    }

    public GachaPoolEntry[] gachaPool; // assign in inspector

    private void Awake()
    {
        instance = this;
    }


    public CatData Roll()
    {
        float totalWeight = 0f;
        foreach (var entry in gachaPool)
            totalWeight += entry.weight;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in gachaPool)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
            {
                return entry.catData;
            }
        }

        return null; // should never happen
    }

    public void Summon()
    {
        CatData rolledCat = Roll();
        if (rolledCat != null)
        {
            // make runtime cat instance
            CatRuntimeData runtimeCat = new CatRuntimeData(rolledCat);

            // add to inventory
            Inventory.instance.Add(rolledCat);

            Debug.Log($"Summoned {rolledCat.itemName}!");
        }
    }
}

