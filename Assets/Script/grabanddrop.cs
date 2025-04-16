using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject expandedVersion; // Reference to the expanded version GameObject

    private Vector3 offset;
    private float zCoord;
    private bool isDragging = false;
    private Vector3 originalScale;
    private Vector3 hoverScale;
    private Vector3 dragScale;
    private Inventory inventory;

    void Start()
    {
        originalScale = transform.localScale;
        hoverScale = originalScale * 1.1f; // Scale up by 10% when hovering
        dragScale = originalScale * 1.2f; // Scale up by 20% when dragging
        inventory = FindObjectOfType<Inventory>();
        SnapGameObjectToInventory(); // Snap to inventory on start

        if (expandedVersion != null)
        {
            expandedVersion.SetActive(false); // Hide the expanded version initially
        }
    }

    void OnMouseEnter()
    {
        if (!isDragging)
        {
            LeanTween.scale(gameObject, hoverScale, 0.1f).setEase(LeanTweenType.easeOutQuad);
            if (expandedVersion != null)
            {
                expandedVersion.SetActive(true); // Show the expanded version
                expandedVersion.transform.localScale = Vector3.zero; // Start from zero scale
                LeanTween.scale(expandedVersion, new Vector3(2, 2, 0), 0.2f).setEase(LeanTweenType.easeOutBack); // Animate to full scale
            }
        }
    }

    void OnMouseExit()
    {
        if (!isDragging)
        {
            LeanTween.scale(gameObject, originalScale, 0.1f).setEase(LeanTweenType.easeOutQuad);
            if (expandedVersion != null)
            {
                LeanTween.scale(expandedVersion, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
                {
                    expandedVersion.SetActive(false); // Hide the expanded version after animation
                });
            }
        }
    }

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
        LeanTween.scale(gameObject, dragScale, 0.1f).setEase(LeanTweenType.easeOutQuad);
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPos() + offset;
            LeanTween.move(gameObject, targetPosition, 0.1f).setEase(LeanTweenType.easeOutQuad);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        LeanTween.scale(gameObject, originalScale, 0.1f).setEase(LeanTweenType.easeOutQuad);

        // Snap to nearest slot
        Vector3 nearestSlotPosition = inventory.GetNearestSlotPosition(transform.position);
        LeanTween.move(gameObject, nearestSlotPosition, 0.1f).setEase(LeanTweenType.easeOutQuad);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void SnapGameObjectToInventory()
    {
        Vector3 nearestSlotPosition = inventory.GetNearestSlotPosition(transform.position);
        LeanTween.move(gameObject, nearestSlotPosition, 0.1f).setEase(LeanTweenType.easeOutQuad);
    }
}