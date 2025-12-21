using UnityEngine;

public class DisplayItemManager : MonoBehaviour
{
    public static DisplayItemManager instance;

    public DisplayItemUI displayItemUI;
    [SerializeField] private Vector2 offset;

    private void Start()
    {
        instance = this;
    }

    public void ShowDisplayUI(RectTransform location)
    {
        RectTransform displayRect = displayItemUI.GetComponent<RectTransform>();
        RectTransform canvasRect = displayRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Start from the hovered element’s screen position
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, location.position);

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

        displayItemUI.gameObject.SetActive(true);
    }


    public void HideDisplayUI()
    {
        displayItemUI.gameObject.SetActive(false);
    }
}
