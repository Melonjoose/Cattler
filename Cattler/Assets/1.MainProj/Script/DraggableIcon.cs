using System;
using UnityEngine;
using UnityEngine.EventSystems; // For drag/drop interfaces

public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // References
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    [HideInInspector] public SlidingIcons slidingIcons; // Reference to manager
    [HideInInspector] public bool isDragging = false;

    // Index in SlidingIcons list
    [SerializeField] public int iconIndex = 0;

    private CatPosition assignedCat;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Find parent canvas for proper scaling
        canvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        // Optional: auto-register with manager if not assigned manually
        if (slidingIcons != null && !slidingIcons.icons.Contains(this))
        {
            slidingIcons.icons.Add(this);
            slidingIcons.UpdateIconIndex(); // Sync index
        }
    }

    // --------------------
    // Drag Handlers
    // --------------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.alpha = 0.6f;          // Semi-transparent
        canvasGroup.blocksRaycasts = false; // Allow drop targets to receive raycasts
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (slidingIcons != null) //Snaps to sliding icon positions.
        {
            slidingIcons.SnapDraggedIcon(this);
        }
    }

    // --------------------
    // Index management
    // --------------------

    public void SetIndex(int newIndex) => iconIndex = newIndex;
    public int GetIndex() => iconIndex;

    public void AssignCatIndex(int catIndex)
    {
        catIndex = iconIndex; // Sync icon index with cat position index
    } 
    public CatPosition GetAssignedCat() => assignedCat;

}
