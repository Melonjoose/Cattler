using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    public bool tutorialButtonPressed = false;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            // Always remove first to prevent duplicates, then add
            button.onClick.RemoveListener(TutorialHighlightTriggered);
            button.onClick.AddListener(TutorialHighlightTriggered);
        }
    }

    public void TutorialHighlightTriggered()
    {
        if (Tutorial.instance != null && Tutorial.instance.inTutorial)
        {
            tutorialButtonPressed = true;

            // Notify Tutorial sequence
            Tutorial.instance.OnTutorialButtonPressed(this);
        }
    }
}