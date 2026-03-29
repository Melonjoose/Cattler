using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]

public class Highlights
{
    public GameObject highlight;
    public CanvasGroup canvasGRP;
}

public class Tutorial : MonoBehaviour
{
    public bool inTutorial = false;
    public static Tutorial instance;
    public Canvas canvas;
    public GameObject blackPanel; //darkpanel to darken the BG.
    public Highlights[] highlights;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        canvas.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            OpeningScene();
        }

    }

    public void OpeningScene()
    {
        inTutorial = true;
        //Fade in from black
        Debug.Log("OpeningScene playing");
        Transition.instance.FadeIn();
        StartCoroutine(OpeningSceneSequence());
    }
    
    IEnumerator OpeningSceneSequence()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.BeginTalk(0);

    }

    public void ShowHighlight(int index)
    {
        canvas.gameObject.SetActive(true);
        blackPanel.gameObject.SetActive(true);
        foreach (var highlight in highlights)
        {
            highlight.highlight.SetActive(false);
            highlight.canvasGRP.interactable = true; //disable all interable inside of highlights
        }
        highlights[index].highlight.SetActive(true);
        EnableSpecificButton(index);
    }

    public void HideAllHighlights()
    {
        foreach(var highlight in highlights)
        {
            highlight.highlight.SetActive(false);
            highlight.canvasGRP.interactable = true; //disable all interable inside of highlights
        }
        canvas.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
    }
    public void EnableSpecificButton(int index)
    {
        foreach (var highlight in highlights)
        {
            highlight.canvasGRP.interactable = false; //disable all interable inside of highlights
        }
        highlights[index].canvasGRP.interactable = true;
    }

    //introduce Summonbutton.
    //talks about earning ink and cores to summon cats.
    //tell players to summon cats.
    //once they summon. force them to watch the animation.
    //tell them to exit out of summon page.
    //when enter lobby, highlight inventory page and tell them to go to the inventory.
    //upon entering , highlight first Icon and team list 1. tell them to drag and drop cat into team slot to equip them.

    public void OnTutorialButtonPressed(TutorialButton button) 
    {
        if( button != null)
        {
            Debug.Log("Tutorial button pressed: " + button.name);

            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            //Debug.Log("CanvasGroup: " + canvasGroup);
            //Debug.Log("Highlight[0].canvasGRP: " + highlights[0].canvasGRP);

            if ( canvasGroup == highlights[0].canvasGRP) // if summoned is pressed while highlighted,
            {
                HideAllHighlights();
                //tell player to summon one cat
                ShowHighlight(1); //highlight summon button

            }

            if(canvasGroup == highlights[1].canvasGRP)
            {
                HideAllHighlights();
                ShowHighlight(2);
            }
        }
    }


    //BEFORE THIS IS TitleScreen 
    // Fade into Lobby.. ///Trigger on start of game....
    // Player: "Where am I???"
    // Player: "My head.. . it hurts..."
    // you feel something heavy on your chest..
    // Timmy Appears cutely into scene.
    // Timmy: "Mistress! He is awake!"
    // Catkeeper appears..
    // Catkeeper: "Good, you're finally awake. We found you unconsious in the Sanctum Forest"
    // Boss: "Been lazying around and freeloading on our hospitality huh?"
    // Boss: "Our guest over here can finally get up and leave."
    // Catkeeper: "Zeke, sweetie! Don't be so rude! It's been ages since we saw another human"
    // Boss: "Hmph. Humans are the worst. You are exception my mistress. But, They are the reason we are in this mess."
    // Boss: "I won't ever forgive what they have done to y.."
    // Loud explosion and rattle..
    // Timmy: "Trouble! Trouble! Inklers have found us! They are on their way to our car!
    // Boss: "Then quickly get us out of here! We are in a car for christ sake!"
    // Timmy: "The thing is.. They blew off one of our tires and we are grounded here until we repair it!"
    // Boss: "Oh for christ sake! Why didn't you start with that. All cats to battle position! Buy me time to repair the fort!"
    //CutSceneOne Ends. //DialogueManager.cs trigger Tutorial.cs

    //tutorial on navigating lobby.
    //tutorial on summoning a new cat. (First summon will always be Timmy the rookie)
    //tutorial on equipping cats to team.
    //tutorial on entering battle.

    //Tutorial Stage. Survive for 1 minute. //toggle travelmanager movement off so they won't walk. set up timer to count 60second miletstone to trigger next event.

    //Finish 1 minute milestone. 
    // Boss: "The car is repaired! All of you, get back in here! We're retreating NOW!"

    //tutorial on retreating.


    //CatKeeper: "That was a close call! We managed to shake them off didn't we?"
    //Boss: "Darn inklings, they've never been this deep into sanctum forest before. Why now?"
    //Timmy: "Boss! Where are we heading now?"
    //Boss: "I guess we should head over to The ruined city for supplies. We are running low on supplies for our mistress."
    //CatKeeper: "Thank you for your hardwork as always, sweetie."
    //Boss: "YOU. I want you to assist us on this task, You proved your worth during that ambush."

    //tutorial end. Mission: Get to 10km.

    public void CutSceneOne()
    {

    }


    ///---Sequence of tutorial steps---///
    ///1. Opening scene. Player gets saved by cats.
    ///2. Sent to Lobbypage.
    ///3. Dialogue system appears.
    ///4. showing Catkeeper talking to player.
    ///5. Sudden screen shake and sound effect. base is attacked.
    ///6. tutorial to set up team. black everything and highlight team buttons.
    ///7. guide player to set up team. drag and drop cats into team slots.
    ///8. guide them back to lobby page.
    ///9. guide them to press start mission.
    ///10. Spawn about 4 or 5 easy enemies.
    ///11. CatKeeper talks and tell player that cats will automatically fight incoming enemies.
    ///12. Special enemy appears
}
