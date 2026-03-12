using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
public class DisplayItemManager : MonoBehaviour
{
    public static DisplayItemManager instance;

    public DisplayItemUI commonUI,rareUI,legendaryUI;
    [SerializeField] private DisplayItemUI chosenUI;
    [SerializeField] private Vector2 offset;

    private void Start()
    {
        instance = this;
    }

    void RarityChecker(CatUnit catUnit)
    {
        if (catUnit.runtimeData.template.Rarity == Rarity.Common)
        {
            chosenUI = commonUI;
            commonUI.gameObject.SetActive(true);
            rareUI.gameObject.SetActive(false);
            legendaryUI.gameObject.SetActive(false);
        }
        else if (catUnit.runtimeData.template.Rarity == Rarity.Rare)
        {
            chosenUI = rareUI;
            commonUI.gameObject.SetActive(false);
            rareUI.gameObject.SetActive(true);
            legendaryUI.gameObject.SetActive(false);
        }
        else if (catUnit.runtimeData.template.Rarity == Rarity.Legendary)
        {
            chosenUI = legendaryUI;
            commonUI.gameObject.SetActive(false);
            rareUI.gameObject.SetActive(false);
            legendaryUI.gameObject.SetActive(true);
        }

    }


    public void ShowDisplayUI(GameObject item)
    {
        CatUnit catUnit = item.GetComponent<CatUnit>();
        RarityChecker(catUnit);

        RectTransform displayRect = chosenUI.GetComponent<RectTransform>();
        RectTransform canvasRect = displayRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Start from the hovered element’s screen position
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, item.transform.position);

        // Add small offset
        Vector2 targetPos = screenPos + offset;

        // Convert from screen > canvas local space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            targetPos,
            null, // null for Screen Space Overlay
            out localPoint
        );

        // Clamp within canvas rect so it doesn't go offscreen
        Vector2 halfSize = displayRect.sizeDelta * 0.5f;
        float minX = -canvasRect.sizeDelta.x / 2 + halfSize.x;
        float maxX = canvasRect.sizeDelta.x / 2 - halfSize.x;
        float minY = -canvasRect.sizeDelta.y / 2 + halfSize.y;
        float maxY = canvasRect.sizeDelta.y / 2 - halfSize.y;

        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
        localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

        // Apply final position
        displayRect.localPosition = localPoint;

        chosenUI.gameObject.SetActive(true);

        chosenUI.Show(item);
    }


    public void HideDisplayUI()
    {
        chosenUI.gameObject.SetActive(false);
    }
}
