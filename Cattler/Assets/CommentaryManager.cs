using System.Collections;
using TMPro;
using UnityEngine;

public class CommentaryManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static CommentaryManager instance;
    public TextMeshProUGUI text;
    public GameObject textBox;  //the gameobject that holds the text.

    public float textTypingSpeed = 1.0f; //how fast the type writing effect is going to be

    public float dialogueLifetime = 5.0f; // the time it stays open before it close.
    //create a list that holds string(text or comment)
    public string[] dialogueTextChoices;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // test
        {
            BeginTalk(0);
        }
    }

    void BeginTalk(int TextChoice)
    {
        //called when catkeeper starts talking.
        textBox.SetActive(true);
        string chosenDialogue = dialogueTextChoices[TextChoice];
        text.text = chosenDialogue;

        StopAllCoroutines();
        StartCoroutine(TypeWritingEffect(chosenDialogue));

    }

    void CloseDialogue()
    {
        textBox.SetActive(false);
    }

    IEnumerator TypeWritingEffect(string dialogue)
    {
        text.text = "";
        foreach (char c in dialogue)
        {
            text.text += c; // add 1 character
            yield return new WaitForSeconds(textTypingSpeed);
        }

        yield return new WaitForSeconds(dialogueLifetime);
        CloseDialogue();
    }

    //---- Triggers ---// 

    //void DetectLowHealth

    void DetectLowHPCat()
    {
        //Find all cat in - game.
        //access their current HP and Max HP
        //if any cat health is below 25%. BeginTalk(1)
    }
}
