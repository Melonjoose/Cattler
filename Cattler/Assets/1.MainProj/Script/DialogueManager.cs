using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public TextMeshProUGUI text;
    public GameObject textBox;  //the gameobject that holds the text.

    public float textTypingSpeed = 1.0f; //how fast the type writing effect is going to be
    public RawImage CharacterLeft;
    public RawImage CharacterRight;

    //create a list that holds string(text or comment)
    //need a list that holds multiple dialogue choices,sprites and scenerios.(screenshake, emergency, shockCharacterLeft, shock characterRight) 
    public string[] dialogueTextChoices;

    public List<string> dialogueQueue = new List<string>();

    public bool isTalking = false;

    //while talking, tap to skip to the end of the text.
    //while end of text, tap to move to next dialogue in queue.

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        textBox.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // test
        {
            BeginTalk(0);
        }
    }

    public void BeginTalk(int TextChoice)
    {
        //called when catkeeper starts talking.
        isTalking = true;
        textBox.SetActive(true);
        string chosenDialogue = dialogueTextChoices[TextChoice];
        text.text = chosenDialogue;

        StopAllCoroutines();
        StartCoroutine(TypeWritingEffect(chosenDialogue));

    }

    void CloseDialogue()
    {
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

        //if mouse is clicked, move to next text in queue else. close dialogue
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
    // Function to start a dialogue sequence
}
