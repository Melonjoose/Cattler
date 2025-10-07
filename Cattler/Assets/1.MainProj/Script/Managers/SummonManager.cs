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
                return entry.catData;
        }

        return null; // should never happen
    }

    public void Summon()
    {
        // check if there is at least one empty slot
        /*
        if (!Inventory.instance.inventoryList.Contains(null))
        {
            Debug.Log("Inventory full! Cannot summon more cats.");
            return;
        }*/

        CatData rolledCat = Roll();
        if (rolledCat == null)
        {
            Debug.LogError("No cat was rolled!");
            return;
        }

        CatRuntimeData runtimeCat = new CatRuntimeData(rolledCat);

        //  2. Add runtime cat to inventory
        Inventory.instance.Add(runtimeCat);


        TeamManager.instance.AddCatToWorld(runtimeCat);


        Debug.Log($"Summoned {rolledCat.itemName}!");
    }
}
