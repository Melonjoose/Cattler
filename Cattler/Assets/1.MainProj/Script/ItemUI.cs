using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public Item itemData;
    public UnityEngine.UI.Image icon;

    public void SetItem(CatRuntimeData item)
    {
        itemData = item;
        icon.sprite = item.unitIcon; // assuming Item has unitIcon
    }

}