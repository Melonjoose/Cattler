using UnityEngine;

public class AtkSpdCrystal : ConsumableItem
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

        float valueAdded = consumableData.fvalue; //atkspd uses the fvalue

        cat.runtimeData.attackSpeed += valueAdded;

        Debug.Log($"{cat.name} gained +{valueAdded} {consumableData.itemNameShort}!");
    }
    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;
        string valueText = consumableData.fvalue.ToString("F1");
        popup.SetText(valueText, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}
