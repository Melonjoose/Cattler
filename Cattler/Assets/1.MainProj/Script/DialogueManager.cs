using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string name;
    public Sprite sprite;
    public bool inLeftPosition = false;
    public bool inRightPosition = false;
}

[System.Serializable]
public class DialogueLine
{
    public Character speaker;          // Who is speaking
    [TextArea(2, 5)]
    public string line;                // What they say
}

[System.Serializable]
public class DialogueSequence
{
    public List<DialogueLine> lines = new List<DialogueLine>();

}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Canvas canvas;
    public TextMeshProUGUI text;
    public GameObject textBox;  //the gameobject that holds the text.

    public float textTypingSpeed = 1.0f; //how fast the type writing effect is going to be
    public TextMeshProUGUI nameText;
    public Image CharacterLeft; //image slot left
    public Image CharacterRight; //image slot right
    public float readingTime = 7f;

    public bool intro1Start = false;
    public bool intro1Completed = false;

    //create a list that holds string(text or comment)
    //need a list that holds multiple dialogue choices,sprites and scenerios.(screenshake, emergency, shockCharacterLeft, shock characterRight) 

    [TextArea(2, 5)]
    public string[] dialogueTextChoices;

    [SerializeField]
    public List<string> dialogueQueue = new List<string>();
    public int dialogueCount = 0;

    public bool isTalking = false;

    public Character[] characters;              // array of characters
    public DialogueSequence[] dialogueSequences; // array of dialogue sequences

    //while talking, tap to skip to the end of the text.
    //while end of text, tap to move to next dialogue in queue.

    private bool firstRetreat = false;
    private bool retreatCompleted = false;
    private bool backToLobby = false;
    private bool backToLobbyCompleted = false;

    public bool deathDialoguePlay = false;
    public bool deathDialogueEnd = false;

    private void Awake()
    {
        instance = this;
        canvas.gameObject.SetActive(true);
    }

    public void Start()
    {
        canvas.gameObject.SetActive(false);
        textBox.SetActive(false);
        CharacterLeft.gameObject.SetActive(false);
        CharacterRight.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // test
        {
            //BeginTalk(0);
        }

        if (Input.GetMouseButtonDown(0)) // left click or tap
        {
            if (isTyping)
            {
                // Skip typing animationn
                isTyping = false;
            }
        }

    }

    public void BeginTalk(int dialogueSeqIndex)
    {
        canvas.gameObject.SetActive(true);
        dialogueCount = 0;  // reset every time you start a new sequence
        DialogueSequence seq = dialogueSequences[dialogueSeqIndex];
        CheckForTrigger(dialogueSeqIndex);

        textBox.SetActive(true);
        StopAllCoroutines();
        dialogueQueue.Clear(); // clear old lines
        AddDialogueToQueue(seq);
    }
    public void AddDialogueToQueue(DialogueSequence dialogueSeq)
    {
        foreach (DialogueLine lines in dialogueSeq.lines)
        {
            dialogueQueue.Add(lines.line);
        }

        // If not currently talking, start immediately
        if (!isTalking)
        {
            isTalking = true;

            BeginTalkFromQueue(dialogueSeq);
        }
    }
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    IEnumerator TypeWritingEffect(string dialogue, Character speaker, DialogueSequence dialogueSeq)
    {
        isTyping = true;
        text.text = "";

        foreach (char c in dialogue)
        {
            text.text += c;

            // If skip pressed, break immediately
            if (!isTyping)
            {
                text.text = dialogue; // show full line
                break;
            }

            yield return new WaitForSeconds(textTypingSpeed);
        }

        isTyping = false;

        // Wait for reading time OR skip
        float timer = 0f;
        while (timer < readingTime)
        {
            if (!isTalking) yield break; // dialogue closed
            if (Input.GetMouseButtonDown(0)) break; // skip reading time
            timer += Time.deltaTime;
            yield return null;
        }

        CloseDialogue();
        BeginTalkFromQueue(dialogueSeq);
    }
    public void CheckForTrigger(int dialogueSeqIndex)
    {

        if (dialogueSeqIndex == 0 && Tutorial.instance.inTutorial == true)
        {
            intro1Start = true;
        }
        if (dialogueSeqIndex == 1 && Tutorial.instance.inTutorial == true)
        {
            firstRetreat = true;
        }
        if (dialogueSeqIndex == 2 && Tutorial.instance.inTutorial == true)
        {
            backToLobby = true;
        }
        if (dialogueSeqIndex == 3) //for death
        {
            deathDialoguePlay = true;
        }
    }

    public void BeginTalkFromQueue(DialogueSequence dialogueSeq)
    {
        if (dialogueQueue.Count == 0 || dialogueCount >= dialogueSeq.lines.Count)
        {
            EndDialogueSequence();
            return;
        }

        isTalking = true;
        textBox.SetActive(true);

        DialogueLine currentLine = dialogueSeq.lines[dialogueCount];
        nameText.text = currentLine.speaker.name;

        LeanTween.scale(nameText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.8f)
                 .setEasePunch();

        string nextDialogue = dialogueQueue[0];
        dialogueQueue.RemoveAt(0);

        StopAllCoroutines();
        StartCoroutine(TypeWritingEffect(nextDialogue, currentLine.speaker, dialogueSeq));
        ShowSpeaker(currentLine.speaker);

        dialogueCount++;
    }
    // Function to start a dialogue sequence

    public void ShowSpeaker(Character character)
    {
        Vector3 defaultScale = new Vector3(1f, 1f, 1f);
        Vector3 largerScale = new Vector3(1.4f, 1.4f, 1.4f);

        Vector3 negdefaultScale = new Vector3(-1f, 1f, 1f);
        Vector3 neglargerScale = new Vector3(-1.4f, 1.4f, 1.4f);

        if (character.sprite == null)
        {
                CharacterLeft.gameObject.SetActive(false);
                CharacterRight.gameObject.SetActive(false);
            return;
        }


        if (character.inLeftPosition)
        {
            CharacterLeft.gameObject.SetActive(true);
            CharacterLeft.sprite = character.sprite;

            // Animate scale up then back down
            LeanTween.scale(CharacterLeft.gameObject, largerScale, 0.8f)
                     .setEasePunch(); // gives a nice "pop" effect

            CharacterLeft.color = Color.white;

            // Reset right side
            LeanTween.scale(CharacterRight.gameObject, negdefaultScale, 0.8f);
            CharacterRight.color = Color.gray;
        }

        if (character.inRightPosition)
        {
            CharacterRight.gameObject.SetActive(true);
            CharacterRight.sprite = character.sprite;

            LeanTween.scale(CharacterRight.gameObject, neglargerScale, 0.8f)
                     .setEasePunch();

            CharacterRight.color = Color.white;

            LeanTween.scale(CharacterLeft.gameObject, defaultScale, 0.8f);
            CharacterLeft.color = Color.gray;
        }
    }

    void CloseDialogue()
    {
        isTalking = false;
        textBox.SetActive(false);
    }

    public void EndDialogueSequence()
    {
        isTalking = false;
        textBox.SetActive(false);
        canvas.gameObject.SetActive(false);
        dialogueQueue.Clear();

        CheckTriggers();
    }

    void CheckTriggers()
    {

        if (intro1Start && Tutorial.instance.inTutorial == true)
        {
            intro1Start = false;
            intro1Completed = true;
            Tutorial.instance.tutorialStage = 0;
            Tutorial.instance.ShowHighlight(0);
            CommentaryManager.instance.TutorialText(0);
            CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
        }

        if (intro1Completed && firstRetreat && Tutorial.instance.inTutorial == true)
        {
            firstRetreat = false;
            retreatCompleted = true;
            Tutorial.instance.TriggerTUT13();
        }

        if (retreatCompleted && backToLobby && Tutorial.instance.inTutorial == true)
        {
            backToLobby = false;
            backToLobbyCompleted = true;
            Tutorial.instance.TriggerTUT16();
        }

        if (deathDialoguePlay && !deathDialogueEnd) //if play is true, dialogue is played(at the start), and if deathdialogue havent end.
        {
            deathDialogueEnd = true;
            GameManager.instance.RetreatConfirmPressed();
        }
    }

    public void resetDeathTriggers()
    {
        deathDialoguePlay = false;
        deathDialogueEnd = false;
    }
    //tap to skip typinganimation if it is still typing.
    //tap to skip the readingtime.
}
