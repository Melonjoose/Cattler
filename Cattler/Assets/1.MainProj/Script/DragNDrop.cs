using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform[] snapPoints; // Array of potential snap points
    public float snapRadius = 50f; // Radius to detect nearby snap points
    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store the original position when dragging starts
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the UI element as it is dragged
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Find the nearest snap point within the snap radius
        RectTransform nearestPoint = null;
        float nearestDistance = float.MaxValue;

        foreach (var snapPoint in snapPoints)
        {
            float distance = Vector2.Distance(rectTransform.anchoredPosition, snapPoint.anchoredPosition);
            if (distance < snapRadius && distance < nearestDistance)
            {
                nearestPoint = snapPoint;
                nearestDistance = distance;
            }
        }

        // Snap to the nearest point or back to the original position
        if (nearestPoint != null)
        {
            SnapToPosition(nearestPoint.anchoredPosition);
        }
        else
        {
            SnapToPosition(originalPosition);
        }
    }

    private void SnapToPosition(Vector2 targetPosition)
    {
        // Use LeanTween to smoothly move the UI element to the target position
        LeanTween.move(rectTransform, targetPosition, 0.3f).setEase(LeanTweenType.easeOutQuad);
    }
}
