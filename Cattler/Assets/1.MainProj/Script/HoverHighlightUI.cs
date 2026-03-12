using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // this is where Unity's Outline lives

public class HoverHighlightUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    private UnityEngine.UI.Outline outline; // explicitly say UI.Outline

    void Awake()
    {
        outline = GetComponent<UnityEngine.UI.Outline>();

        if (outline == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a UI Outline component!");
        }
        else
        {
            outline.enabled = false; // disable by default
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null) outline.enabled = true;
        AudioManager.instance.PlaySFX("ButtonUI2");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null) outline.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (outline != null) outline.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (outline != null) outline.enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (outline != null) outline.enabled = false;
    }
}