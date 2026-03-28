using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CommentaryManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static CommentaryManager instance;
    public Canvas canvas;
    public GameObject catKeeper; // reference to catkeeper gameobject
    public CanvasGroup catKeeperUI; // reference to catkeeper UI canvasgroup
    public TextMeshProUGUI text;
    public GameObject textBox;  //the gameobject that holds the text.
    public GameObject[] catContainer;

    private bool isTyping = false;
    public float textTypingSpeed = 1.0f; //how fast the type writing effect is going to be
    public float dialogueLifetime = 5.0f; // the time it stays open before it close.
    public float defaultDialogueLifetime = 5.0f; // default
    //create a list that holds string(text or comment)
    public string[] dialogueTextChoices;
    public string[] tutorialTextChoices; // for tutorial

    public List<string> dialogueQueue = new List<string>();
    public GameObject topLeftPosition;  //default
    public GameObject bottomLeftPosition; //secondary

    public bool isTalking = false;

    private void Awake()
    {
        instance = this;
        canvas.gameObject.SetActive(true);
    }

    public void Start()
    {
        catKeeperUI.alpha = 0;
        textBox.SetActive(false);
    }   

    private Coroutine typingCoroutine;

    void OpenCanvasGroup()
    {
        CanvasGroup canvasGroup = catKeeperUI;
        canvasGroup.alpha = 1;
        textBox.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        MoveCommentary(topLeftPosition);
    }

    public void LimitQueue()
    {
        //only remember the 2 latests commentary. delete all old ones.
    }

    public void StopSpam()
    {
       // prevent player from getting spammed.
    }

    IEnumerator TypeWritingEffect(string dialogue ,float lifetime)
    {
        isTyping = true;
        text.text = "";

        foreach (char c in dialogue)
        {
            text.text += c;
            if (!isTyping) // interrupted by click
            {
                text.text = dialogue; // instantly finish
                break;
            }
            yield return new WaitForSeconds(1f / textTypingSpeed);
        }

        isTyping = false;

        yield return new WaitForSeconds(lifetime);
        CloseDialogue();

        if (dialogueQueue.Count > 0)
        {
            BeginTalkFromQueue();
        }
    }


    public void AddDialogueToQueue(int dialogueIndex)
    {
        OpenCanvasGroup();
        // Add the chosen dialogue line to the queue
        string chosenDialogue = dialogueTextChoices[dialogueIndex];
        dialogueQueue.Add(chosenDialogue);

        // If not currently talking, start immediately
        if (!isTalking)
        {
            BeginTalkFromQueue();
        }
        AudioManager.instance.PlaySFX("SoftDeny");
    }

    public void BeginTalkFromQueue()
    {
        if (dialogueQueue.Count == 0) return;

        isTalking = true;
        textBox.SetActive(true);

        string nextDialogue = dialogueQueue[0];
        dialogueQueue.RemoveAt(0);

        StopAllCoroutines();
        if(Tutorial.instance.inTutorial == false)
        {
            StartCoroutine(TypeWritingEffect(nextDialogue, defaultDialogueLifetime));
        }
        else
        {
            StartCoroutine(TypeWritingEffect(nextDialogue, 999f));
        }
    }

    public void ClickOnBoxInteraction()
    {
        //when click on box, this plays.
        if (isTyping) //if still typing,  skip dialogue and show full text.
        {
            //show full text immediately.
            isTyping = false; // instantly finish typing
            return;
        }

        if (dialogueQueue.Count > 0)
        {
            BeginTalkFromQueue();
            return;
        }

        CloseDialogue();
    }
    public void CloseDialogue()
    {
        isTalking = false;
        catKeeperUI.alpha = 0;
        CanvasGroup canvasGroup = catKeeperUI;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        textBox.SetActive(false);
        StopAllCoroutines();
    }

    //Tutorial Usage below//

    public void TutorialText(int TextChoice)
    {
        Debug.Log("tutorialComment" + TextChoice);
        OpenCanvasGroup();
        // Add the chosen dialogue line to the queue
        string chosenDialogue = tutorialTextChoices[TextChoice];
        dialogueQueue.Add(chosenDialogue);

        // If not currently talking, start immediately
        if (!isTalking)
        {
            BeginTalkFromQueue();
        }

    }

    public void MoveCommentary(GameObject locationGO)
    {
        catKeeperUI.transform.position = locationGO.gameObject.transform.position;
    }
    //---- Triggers ---// 

    //Called from other scripts to trigger dialogue.


}
