using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N)) // test
        //{
        //    BeginTalk(0);
        //}
    }

    public void BeginTalk(int TextChoice)
    {
        //called when catkeeper starts talking.
        catKeeperUI.alpha = 1;
        isTalking = true;
        textBox.SetActive(true);
        string chosenDialogue = dialogueTextChoices[TextChoice];
        text.text = chosenDialogue;

        StopAllCoroutines();
        StartCoroutine(TypeWritingEffect(chosenDialogue));

    }

    void CloseDialogue()
    {
        catKeeperUI.alpha = 0;
        isTalking = false;
        textBox.SetActive(false);
    }

    IEnumerator TypeWritingEffect(string dialogue)
    {
        text.text = "";
        foreach (char c in dialogue)
        {
            text.text += c;
            yield return new WaitForSeconds(textTypingSpeed);
        }

        yield return new WaitForSeconds(dialogueLifetime);
        CloseDialogue();

        // If more dialogues are queued, continue automatically
        if (dialogueQueue.Count > 0)
        {
            BeginTalkFromQueue();
        }
    }


    public void AddDialogueToQueue(int dialogueIndex)
    {
        // Add the chosen dialogue line to the queue
        string chosenDialogue = dialogueTextChoices[dialogueIndex];
        dialogueQueue.Add(chosenDialogue);

        // If not currently talking, start immediately
        if (!isTalking)
        {
            BeginTalkFromQueue();
        }
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


    //---- Triggers ---// 

    //Called from other scripts to trigger dialogue.


}
