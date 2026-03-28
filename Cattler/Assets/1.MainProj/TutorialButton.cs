using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public bool tutorialButtonPressed = false;

    public void TutorialHighlightTriggered()
    {
        if(Tutorial.instance.inTutorial == true)
        {
            tutorialButtonPressed = true;

            // Notify Tutorial sequence
            Tutorial.instance.OnTutorialButtonPressed(this);
            CommentaryManager.instance.CloseDialogue();
            CommentaryManager.instance.dialogueQueue.Clear();
        }
    }
}
