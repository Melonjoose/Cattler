using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public ItemData itemData;
    public Image iconImage;

    private void Awake()
    {
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();
    }
    public void SetItem(ItemData item)
    {
        if (item == null || iconImage == null) return;

        itemData = item;
        iconImage.sprite = item.icon;
        Debug.Log($"Set item UI to {item.itemName}");
    }

}
