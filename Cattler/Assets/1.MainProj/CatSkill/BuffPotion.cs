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

        BuffManager.instance.ApplyBuff( cat.gameObject, BuffManager.instance.attackSpeedUp , 5f, BuffManager.instance.attackSpeedUp.value);

        Debug.Log($"{cat.name} buffed +{buffValue} AttackSpeed for 5s!");
    }


    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;

        string valueText = (BuffManager.instance.attackSpeedUp.value).ToString("F0") + "%";
        popup.SetText(valueText, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }
}