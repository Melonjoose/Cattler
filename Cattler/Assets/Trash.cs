using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Trash : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public static Trash instance;

    [SerializeField] private CanvasGroup canvasrgp;
    [SerializeField] private Image image;
    [SerializeField] private GameObject textGO;

    [Header("Sprites")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    void Awake()
    {
        instance = this;

        if (canvasrgp == null) canvasrgp = GetComponent<CanvasGroup>();
        if (image == null) image = GetComponent<Image>();

        if (textGO == null)
        {
            TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
                textGO = textComponent.gameObject;
        }

        // Default state
        if (image != null && closedSprite != null)
            image.sprite = closedSprite;
            textGO.SetActive(false);
    }

    // Hover enter > open bin
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null && openSprite != null)
            image.sprite = openSprite;
        textGO.SetActive(true);
    }

    // Hover exit > close bin
    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null && closedSprite != null)
            image.sprite = closedSprite;
        textGO.SetActive(false);
    }

    // Drop handler > delete item
    public void OnDrop(PointerEventData eventData)
    {
        // The dragged object is eventData.pointerDrag
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null)
        {
            Debug.Log($"Dropped {droppedItem.name} into trash!");

            // Tell Inventory to remove it properly
            Inventory.instance.Remove(droppedItem, null);

            // Destroy the GameObject itself
            Destroy(droppedItem);
        }

        // Reset trash bin sprite
        if (image != null && closedSprite != null)
            image.sprite = closedSprite;
    }
}