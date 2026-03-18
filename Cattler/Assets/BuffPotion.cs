using System.Collections;
using UnityEngine;

public class BuffPotion : ConsumableItem
{
    public int valuePercentage = 20;

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

    [SerializeField]private float buffValue;

    private void Use()
    {
        if (cat == null || consumableData == null) return;

        buffValue = (cat.runtimeData.attackSpeed * valuePercentage) / 100f;
        Buff atkSpeedBuff = new Buff("Attack Speed Potion", 5f, StatType.AttackSpeed, buffValue); //apply buff for 5 seconds, atkspd type of value

        BuffManager.instance.ApplyBuff(atkSpeedBuff , cat);

        Debug.Log($"{cat.name} buffed +{buffValue} AttackSpeed for 5s!");
    }


    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;
        popup.SetText(buffValue, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}