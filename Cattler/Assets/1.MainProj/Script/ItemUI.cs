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
            itemData = cat;
            iconImage.sprite = cat.unitIcon;
        }
    }
}
