using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        CommentaryManager.instance.ClickOnBoxInteraction();
    }
}
