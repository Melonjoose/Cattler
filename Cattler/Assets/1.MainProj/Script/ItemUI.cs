using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Item itemData;
    public Image iconImage;

    public void SetItem(CatRuntimeData cat)
    {
        if (cat != null && iconImage != null)
        {
            itemData = cat; // store as Item (since CatRuntimeData : Item)
            iconImage.sprite = cat.unitIcon;
        }
    }

    public CatRuntimeData GetCatData()
    {
        return itemData as CatRuntimeData; // safe cast back
    }
}
