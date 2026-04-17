using UnityEngine;

public class MovementSpdCrystal : ConsumableItem
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

        cat.runtimeData.attackPower += valueAdded;

        Debug.Log($"{cat.name} gained +{valueAdded} {consumableData.itemNameShort}!");
    }
    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;
        string valueText = consumableData.value.ToString();
        popup.SetText(valueText, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}
