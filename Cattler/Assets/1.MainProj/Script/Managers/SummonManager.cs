using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance;
    public GameObject catTemplatePrefab;

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
        if(Inventory.instance.items.Count >= Inventory.instance.currentCapacity) return;

        CatData rolledCat = Roll();
        if (rolledCat == null) return;

        //  1. Create a runtime instance of the cat
        CatRuntimeData runtimeCat = ScriptableObject.CreateInstance<CatRuntimeData>();
        runtimeCat.InitializeFrom(rolledCat); // copy stats from template

        //  2. Add runtime cat to inventory
        Inventory.instance.Add(runtimeCat);

        //  3. Instantiate the prefab in the scene
        GameObject newCatGO = Instantiate(catTemplatePrefab);
        CatUnit catUnit = newCatGO.GetComponent<CatUnit>();
        catUnit.AssignCat(runtimeCat);

        Debug.Log($"Summoned {rolledCat.unitName}!");

        //add cat to inventory UI.

        TeamManager.instance.AddCatToTeam(catUnit);
    }
}
