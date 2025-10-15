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

        Debug.Log($"{cat.name} healed +{healthToRecover} Health!");
    }
    private void VFX()
    {
        SFX popup = SFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;
        popup.SetText(healthToRecover, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}
