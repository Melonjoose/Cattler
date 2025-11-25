using UnityEngine;

public class HealthCrystal : ConsumableItem
{
    protected override void Start()
    {
        base.Start();
        onConsumed += Use;
        onConsumed += VFX;
    }
    private void OnDestroy()
    {
        onConsumed -= Use;
        onConsumed -= VFX;
    }
    private void Use()
    {
        if (cat == null || consumableData == null) return;

        int valueAdded = consumableData.value;

        cat.runtimeData.maxHealth += valueAdded;

        cat.runtimeData.currentHealth = Mathf.Min(
            cat.runtimeData.currentHealth + valueAdded,
            cat.runtimeData.maxHealth

        );

        int currentHealth = cat.runtimeData.currentHealth;
        int maxHealth = cat.runtimeData.maxHealth;
        cat.OnHealthChange(currentHealth, maxHealth);

        Debug.Log($"{cat.name} gained +{valueAdded} max HP!");
    }
    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;
        popup.SetText(consumableData.value, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}
