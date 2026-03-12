using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CommentaryManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static CommentaryManager instance;
    public GameObject catKeeper; // reference to catkeeper gameobject
    public CanvasGroup catKeeperUI; // reference to catkeeper UI canvasgroup
    public TextMeshProUGUI text;
    public GameObject textBox;  //the gameobject that holds the text.
    public GameObject[] catContainer;

    private bool isTyping = false;
    public float textTypingSpeed = 1.0f; //how fast the type writing effect is going to be
    public float dialogueLifetime = 5.0f; // the time it stays open before it close.
    //create a list that holds string(text or comment)
    public string[] dialogueTextChoices;

    public List<string> dialogueQueue = new List<string>();


    public bool isTalking = false;

    private void Awake()
    {
        instance = this;
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
    }

    public void BeginTalk(int TextChoice)
    {
        string chosenDialogue = dialogueTextChoices[TextChoice];

        isTalking = true;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeWritingEffect(chosenDialogue));
    }


    IEnumerator TypeWritingEffect(string dialogue)
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

        yield return new WaitForSeconds(dialogueLifetime);
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
        StartCoroutine(TypeWritingEffect(nextDialogue));
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
    void CloseDialogue()
    {
        isTalking = false;
        catKeeperUI.alpha = 0;
        CanvasGroup canvasGroup = catKeeperUI;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        textBox.SetActive(false);
        StopAllCoroutines();
    }
    //---- Triggers ---// 

    //Called from other scripts to trigger dialogue.


}
