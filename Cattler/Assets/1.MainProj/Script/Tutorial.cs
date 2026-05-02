using System.Collections;
using UnityEngine;
[System.Serializable]

public class Highlights
{
    public GameObject highlight;
    public CanvasGroup canvasGRP;
}

[System.Serializable]
public class TutorialLevel
{
    //player needs to survive for 30 seconds. spawn some easy enemies to attack the player. player needs to survive until the end of the timer.
    public float timer;
    public bool paused = false;
    public float timeToSurvive = 30f;

    //milestone for tutorial.
    public float timerCheckpoint1 = 10f; //to trigger highlighting of the movement controls. "You can drag and drop cats to re-position them in battle"
    public bool checkpoint1Reached = false; 
    public bool checkpoint1Completed = false;
    public Level leveltoSpawnArty;

    public bool istutorialLevelActive = false;
    public bool isLevelCompleted = false;
}

public class Tutorial : MonoBehaviour
{
    public int tutorialStage = 0;
    public bool inTutorial = false;
    public static Tutorial instance;
    public Canvas canvas;
    public Canvas skipTutorialCanvas;
    public GameObject blackPanel; //darkpanel to darken the BG.
    public Highlights[] highlights;
    public TutorialLevel tutorialLevel;
    public GameObject finishButton; //to be activated when tutorial is completed.

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        skipTutorialCanvas.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
        finishButton.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            OpeningScene();

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            HideAllHighlights();
        }
        if(Input.GetKeyDown(KeyCode.Space) && inTutorial)
        {
            SkipTutorial();
        }

        if(tutorialLevel.istutorialLevelActive && !tutorialLevel.isLevelCompleted )
        {
            if(tutorialLevel.paused == false)
            {
                tutorialLevel.timer += Time.deltaTime; // only runs when tutorial level is active and not completed
            }

            if (tutorialLevel.checkpoint1Reached == true )// once reach the checkpoint1 . pause.
            {
                if(tutorialLevel.checkpoint1Completed == true) //if checkpoint 1 is completed, unpause the timer and resume enemy movement.
                {
                    tutorialLevel.paused = false;
                }
                else if(tutorialLevel.checkpoint1Completed == false) //if checkpoint 1 is not completed, keep paused and stop enemy movement.   
                {
                tutorialLevel.paused = true; 
                }
            }

            Distance.instance.UpdateTimerText(tutorialLevel.timer);

            if(tutorialLevel.timer >= tutorialLevel.timerCheckpoint1 && !tutorialLevel.checkpoint1Reached) //TUT10. MOVEMENT Tutorial.
            {

                //pause the timer and enemy movement until player complete movement tutorial
                TriggerTUT10();
            }


            if (tutorialLevel.timer >= tutorialLevel.timeToSurvive) //TUT12. dialogue. "The car is repaired! All of you, get back in here! We're retreating NOW!"
            {
                TriggerTUT12();
            }
        }
    }

    public void ContinueTutorialButton()
    {
        skipTutorialCanvas.gameObject.SetActive(false);
        OpeningScene();
    }

    public void SkipTutorialButton()
    {
        SkipTutorial();
    }

    public void SkipTutorial() //when click on button, Finish Tutorial!
    {
        inTutorial = false;
        HideAllHighlights();
        StopAllCoroutines();
        CommentaryManager.instance.CloseDialogue();
        DialogueManager.instance.EndDialogueSequence();
        //CommentaryManager.
        canvas.gameObject.SetActive(false);
        skipTutorialCanvas.gameObject.SetActive(false);
    }

    public void OpeningScene()
    {
        inTutorial = true;
        //Fade in from black
        Debug.Log("OpeningScene playing");
        //Transition.instance.FadeIn();
        DialogueManager.instance.BeginTalk(0);
        //StartCoroutine(OpeningSceneSequence());
    }
    
    IEnumerator OpeningSceneSequence()
    {
        yield return new WaitForSeconds(1f);


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
        if (button != null)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup == highlights[0].canvasGRP) // if summon page (catKeeper) is pressed while highlighted, IN LOBBY PAGE > SUMMON PAGE.
            {
                Invoke("TriggerTUT1", 0.2f);
            }
            if (canvasGroup == highlights[1].canvasGRP) //summon button is pressed while highlighted, STILL IN SUMMON PAGE. When summon pressed, summon animation triggers.
            {
                Invoke("TriggerTUT2", 0.2f);
            }

            //TUT3 at SummonManager.cs

            //TUT4 at SummonManager.cs

            if (canvasGroup == highlights[5].canvasGRP) //close button inside of Summonpage.
            {
                Invoke("TriggerTUT5", 0.2f);
            }

            if (canvasGroup == highlights[2].canvasGRP) //if click on inventory button at lobby.
            {
                Invoke("TriggerTUT6", 0.2f);
            }

            //TUT7 in Inventory.cs when drag and drop is successful. highlight close button to back to lobby page.

            if (canvasGroup == highlights[7].canvasGRP) //if button is close button at inventory page.
            {
                Invoke("TriggerTUT8", 0.2f);
            }

            if (canvasGroup == highlights[4].canvasGRP) //if battle door is clicked while highlighted,
            {
                Invoke("TriggerTUT9", 0.2f);
            }

            //TUT10; at update.

            //TUT11 at catMovement.cs

            //TUT12 at update.


            if (canvasGroup == highlights[9].canvasGRP) //if retreat button clicked and is highlighted,
            {
                TriggerTUT14();
            }

            if (canvasGroup == highlights[10].canvasGRP) //if confirm button in retreat page is clicked and is highlighted,
            {
                Invoke("TriggerTUT15", 0.9f);
            }
        }
    }

    public void TriggerTUT1()
    {
        tutorialStage = 1;
        HideAllHighlights();
        //tell player to summon one cat
        ShowHighlight(1); //highlight summon button
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(1); // "Here is free 100 ink from me! You need this to summon for different types of cats. Press the summon button to summon our first defender!"
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
    }

    public void TriggerTUT2()
    {
        tutorialStage = 2;
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        HideAllHighlights(); // player is forced to watch the animation, so hide highlights for now. will show next highlight once animation is done.
    }

    public void TriggerTUT3()
    {
        Tutorial.instance.tutorialStage = 3;
        Tutorial.instance.HideAllHighlights();
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(2); // "Hit the close button to return to head back to the lobby."
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
    }

    public void TriggerTUT4() 
    {
        Tutorial.instance.tutorialStage = 4;// when close summon page is triggered during firstsummon.
                                            //TUT4
                                            //highlight close button.
        Tutorial.instance.HideAllHighlights();
        Tutorial.instance.ShowHighlight(5); //highlight 5 == close button.
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(3); // "Hit the close button to return to head back to the lobby."
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.bottomLeftPosition);
    }

    public void TriggerTUT5()
    {
        //once close button is pressed. go lobby and highlight inventory button.
        tutorialStage = 5;
        //highlight inventory button.
        HideAllHighlights();
        ShowHighlight(2); //highlight inventory button

        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(4); // "We need to go to the inventory to add the cat to your team!"
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.bottomLeftPosition);
    }

    public void TriggerTUT6() 
    {

        //once close button is pressed. go lobby and highlight inventory button.
        tutorialStage = 6;
        //highlight inventory button.
        HideAllHighlights();
        ShowHighlight(6); //highlight inventory button

        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(5); // "We need to go to the inventory to add the cat to your team!"
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.addTeamPosition);
    }

    public void TriggerTUT7()
    {
        Tutorial.instance.tutorialStage = 7;
        //highlight inventory button.
        Tutorial.instance.HideAllHighlights();
        Tutorial.instance.ShowHighlight(7);

        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(6);
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.bottomLeftPosition);
    }

    public void TriggerTUT8()
    {
        //once close button is pressed. go lobby and highlight inventory button.
        tutorialStage = 8;
        //highlight inventory button.
        HideAllHighlights();
        ShowHighlight(4); //highlight battle door button.

        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(7); // "We need to go to the inventory to add the cat to your team!"
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topRightPosition);
    }

    public void TriggerTUT9()
    {
        tutorialStage = 9;
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        HideAllHighlights(); // player is forced to watch the animation, so hide highlights for now. will show next highlight once animation is done.
        Invoke("TutorialLevelStart", 2f); //start the tutorial level after a short delay to allow for any transition animations to complete.
    }
    public void TriggerTUT10()
    {
        tutorialStage = 10;
        tutorialLevel.checkpoint1Reached = true;
        //Trigger checkpoint 1 event. Highlight movement controls.
        ShowHighlight(8); //highlight movement controls.
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(8); // "You can drag and drop cats to re-position them in battle!"
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
        GameManager.instance.FreezeGameplay();
        GameManager.instance.EnableAllCats();
    }

    public void TriggerTUT11() //complete movement tutorial.
    {
        GameManager.instance.ResumeGameplay();
        TravelManager.instance.DisableTravel();
        tutorialStage = 11;
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(9); // reasons to move your cats.
        HideAllHighlights(); // player is forced to watch the animation, so hide highlights for now. will show next highlight once animation is done.
        
        SpecialEnemySpawner.instance.InitalizeLevelToSpawner(tutorialLevel.leveltoSpawnArty);
        tutorialLevel.checkpoint1Completed = true; // mark checkpoint 1 as completed after player learn movement.
    }

    public void TriggerTUT12()
    {
        tutorialStage = 12;
        tutorialLevel.isLevelCompleted = true;
        Debug.Log("Tutorial Level Completed! Survived for 30 seconds.");
        // Trigger next event or dialogue here
        GameManager.instance.FreezeGameplay();
        DialogueManager.instance.BeginTalk(1);
    }

    public void TriggerTUT13() //triggered when dialogue finished.
    {
        tutorialStage = 13;
        HideAllHighlights();
        ShowHighlight(9); //highlight 
        CommentaryManager.instance.CloseDialogue();
        CommentaryManager.instance.TutorialText(10);
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
        //highlight retreat button and press retreat.
        //cat keeper "click on the retreat button!" 
    }

    public void TriggerTUT14()
    {
        tutorialStage = 14;
        HideAllHighlights();
        ShowHighlight(10); //highlight 
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(11);
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
        //cat keeper "this is the retreat page. It shows all your earnings from this run. Let's click on the confirm button to head back to lobby."
    }

    public void TriggerTUT15() 
    {
        tutorialStage = 15;
        HideAllHighlights();

        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        DialogueManager.instance.BeginTalk(2);
        //cat keeper "this is the retreat page. It shows all your earnings from this run. Let's click on the confirm button to head back to lobby."
    }

    public void TriggerTUT16() //triggered when dialogue finished.
    {
        tutorialStage = 16;
        HideAllHighlights();
        ShowHighlight(12); //highlight
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        CommentaryManager.instance.TutorialText(12);
        CommentaryManager.instance.MoveCommentary(CommentaryManager.instance.topLeftPosition);
        finishButton.SetActive(true); //enable the button to activate CompleteTutorial Function.
        //cat keeper "Great job on completing the tutorial! Now let's head over to the ruined city to scavenge for supplies and meet some new friends!"
    }

    public void CompleteTutorial() // activate by finishbutton.
    {
        inTutorial = false;
        HideAllHighlights();
        CommentaryManager.instance.CloseDialogue(); //close previous dialogue first before opening new one.
        Distance.instance.ShowDistance(); //switch back to distance UI.
    }

    public void TutorialLevelStart() //to be activated when game starts.
    {
        tutorialLevel.istutorialLevelActive = true;
        Distance.instance.ShowTimer(); //show timer instead of distance.
        foreach(CatUnit cat in TeamManager.instance.cats)
        {
            cat.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true); //set all cats to idle because they are not travelling.
        }
    }

    public void firstItemDropTutorial() //to be activated when player collect the first item.
    {
        //highlight the item and tell player that they can drag and drop to your cats to collect it. items will be added into your inventory.
    }

    private bool firstItemCollected = false; // to track if the first item has been collected.
    private bool firstItemTutorialCompleted = false; //track completion of tutorial.
    public void EquipItemTutorial()
    {
        //if in lobby. and first item is collected is true. play this. 
        if(firstItemCollected == true)
        {
            if(!firstItemTutorialCompleted)
            {
                //highlight the item and tell player that they can drag and drop to your cats to equip it. items will be added into your inventory.
                firstItemTutorialCompleted = true; // mark tutorial as completed after showing it once.
                //tutorial below.
            }
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

