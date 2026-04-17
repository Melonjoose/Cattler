using UnityEngine;

public class HealingPotion : ConsumableItem
{
    public int HealingPercentage = 30;
    private int healthToRecover;
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

        healthToRecover = (cat.runtimeData.maxHealth * HealingPercentage) / 100;

        cat.runtimeData.currentHealth = Mathf.Min(
            cat.runtimeData.currentHealth + healthToRecover,
            cat.runtimeData.maxHealth
        );

        int currentHealth = cat.runtimeData.currentHealth;
        int maxHealth = cat.runtimeData.maxHealth;
        cat.OnHealthChange(currentHealth, maxHealth);

        Debug.Log($"{cat.name} healed +{healthToRecover} Health!");
    }
    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;
        string valueText = healthToRecover.ToString();
        popup.SetText(valueText, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}
