using Unity.VisualScripting;
using UnityEngine;

public class Coconut : ConsumableItem
{
    public CoconutParty coconutParty;
    public int valuePercentage; //20%

    private void Awake()
    {
        coconutParty = GetComponentInParent<CoconutParty>();
    }

    protected override void Start()
    {
        valuePercentage = consumableData.value;
        base.Start();
        onConsumed += Use;
        onConsumed += VFX;
    }

    private void OnDestroy()
    {
        onConsumed -= Use;
        onConsumed -= VFX;
    }

    [SerializeField] private float buffValue;

    private void Use()
    {
        if (cat == null || consumableData == null) return;

        // Calculate reduction amount (20% of cooldown)
        buffValue = cat.skill.cooldown * (valuePercentage / 100f);

        // Apply cooldown reduction
        cat.skill.time -= buffValue;
    }

    private void VFX()
    {
        StatFX popup = StatFXManager.instance.GetFromPool();
        popup.transform.position = this.transform.position;

        // Show percentage reduction, not raw seconds
        string valueText = valuePercentage.ToString("F0") + "%";
        popup.SetText(valueText, consumableData.itemNameShort);
        popup.SetIcon(consumableData.icon);
    }

}
