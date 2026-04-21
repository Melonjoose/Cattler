using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{

    public int ink = 100; // Ink currency drop by all enemies. Bosses drop more ink than enemies.
    public int eXP = 0; // EXP can be earn after the player retreats. EXP can also be earn from dead cats
    public int core = 0; //Rare currency drop by mini bosses and bosses
    
    public TextMeshProUGUI[] inkText; // Text to display ink amount
    public TextMeshProUGUI[] expText; // Text to display EXP amount
    public TextMeshProUGUI[] coreText; // Text to display core amount
    
    public Animator inkAnimator; // Animator for ink text
    public Animator eXPAnimator;
    public Animator coreAnimator;

    //currency earned in current mission, reset to 0 after retreat or start new mission
    public int inkEarnedThisMission = 0;
    public int coreEarnedThisMission = 0;
    public int itemsEarnedThisMission = 0;

    public static Currency instance; //Singleton instance to access this script easily from other scripts

    //might want to make this script accessible to all other scripts

    void Start()
    {
        instance = this;
        UpdateGUI();
        //Check for save data of currency
    }

    public void AddInk(int amount)
    {

        ink += amount;    
        UpdateGUI(); // Update the GUI after adding ink
        
        if(inkAnimator != null) 
        {
            inkAnimator.SetTrigger("Bounce"); // Trigger bounce animation
        }

        inkEarnedThisMission += amount; // Track ink earned in this mission
        //create a floating green text above the ink or red ink whenever transaction happens.
    }

    public void RemoveInk(int amount)
    {

        ink += amount;
        UpdateGUI(); // Update the GUI after adding ink

        if (inkAnimator != null)
        {
            inkAnimator.SetTrigger("Bounce"); // Trigger bounce animation
        }



        //create a floating green text above the ink or red ink whenever transaction happens.
    }

    public void AddEXP(int amount)
    {
        eXP += amount;
        UpdateGUI(); // Update the GUI after adding EXP
    }

    public void AddCore(int amount)
    {
        core += amount;
        UpdateGUI(); // Update the GUI after adding core
        if(coreAnimator != null )
        {
           coreAnimator.SetTrigger("Bounce"); // Trigger bounce animation
        }

        coreEarnedThisMission += amount; // Track core earned in this mission
    }

    void UpdateGUI()
    {
        foreach (var text in inkText)
        {
            text.text = ink.ToString();
        }
        foreach (var text in expText) 
        { 
            text.text = eXP.ToString(); 
        }
        foreach (var text in coreText)
        {
            text.text = core.ToString();
        }
    }

    public void ResetCurrencyForNewMission()
    {
        inkEarnedThisMission = 0;
        coreEarnedThisMission = 0;
        itemsEarnedThisMission = 0;
    }   
}
