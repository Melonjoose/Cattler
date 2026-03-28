using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject objectToHover;
    [SerializeField] private GameObject textGO;
    [SerializeField] private GameObject highlighter;
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup canvasGroup;

    void Awake()
    {
        objectToHover = this.gameObject;
        highlighter = objectToHover.transform.Find("Highlighter")?.gameObject;
        textGO = objectToHover.transform.Find("Text")?.gameObject;
        animator = this.GetComponent<Animator>();
        canvasGroup = this.GetComponent<CanvasGroup>();

        if (highlighter != null) highlighter.SetActive(false);
        if (textGO != null) textGO.SetActive(false);
        if (animator != null) animator.SetBool("Hovering", false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(canvasGroup.interactable == false) { return; }
        
        if (highlighter != null) highlighter.SetActive(true);
        if (textGO != null) textGO.SetActive(true);
        //set animator hovering bool to true
        if(animator != null) animator.SetBool("Hovering", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canvasGroup.interactable == false) { return; }

        if (highlighter != null) highlighter.SetActive(false);
        if (textGO != null) textGO.SetActive(false);
        if (animator != null) animator.SetBool("Hovering", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canvasGroup.interactable == false) { return; }

        if (highlighter != null) highlighter.SetActive(false);
        if (textGO != null) textGO.SetActive(false);
        if (animator != null) animator.SetBool("Hovering", false);
    }
}