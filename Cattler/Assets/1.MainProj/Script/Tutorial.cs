using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Animator transitionAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OpeningScene();
    }

    public void FadeIn()
    {
        transitionAnimator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        transitionAnimator.SetTrigger("FadeOut");
    }

    public void OpeningScene()
    {
        //Fade in from black
        Debug.Log("OpeningScene playing");
        FadeIn();
        CutSceneOne();
    }


    // First cutscene where player is saved by cats
    // Long Black screen..
    // Fade into Lobby..
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
    //CutSceneOne Ends.
    
    //tutorial on navigating lobby.
    //tutorial on summoning a new cat. (First summon will always be Timmy the rookie)
    //tutorial on equipping cats to team.
    //tutorial on entering battle.

    //Tutorial Stage. Survive for 1 minute.


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
