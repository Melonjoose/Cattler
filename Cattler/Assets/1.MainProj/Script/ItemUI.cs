using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Item itemData;
    public Image iconImage;

    private void Awake()
    {
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();
    }
    public void SetItem(CatRuntimeData cat)
    {
        if (cat == null || iconImage == null) return;

        itemData = cat;
        iconImage.sprite = cat.unitIcon;
        Debug.Log($"Set item UI to {cat.unitName}");
    }

    public CatRuntimeData GetCatData()
    {
        return itemData as CatRuntimeData; // safe cast back
    }
}
