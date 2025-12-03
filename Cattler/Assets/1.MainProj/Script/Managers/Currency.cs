using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{

    public int ink = 0; // Ink currency drop by all enemies. Bosses drop more ink than enemies.
    public int eXP = 0; // EXP can be earn after the player retreats. EXP can also be earn from dead cats
    public int core = 0; //Rare currency drop by mini bosses and bosses
    
    public TextMeshProUGUI inkText; // Text to display ink amount
    public TextMeshProUGUI expText; // Text to display EXP amount
    public TextMeshProUGUI coreText; // Text to display core amount

    public Animator inkAnimator; // Animator for ink text
    public Animator eXPAnimator;
    public Animator coreAnimator;


    public static Currency instance; //Singleton instance to access this script easily from other scripts

    //might want to make this script accessible to all other scripts

    void Start()
    {
        instance = this;
        //Check for save data of currency
    }

    public void AddInk(int amount)
    {
        if (amount <= 0) return;

        ink += amount;    
        UpdateGUI(); // Update the GUI after adding ink
        
        if(inkAnimator != null) 
        {
            inkAnimator.SetTrigger("Bounce"); // Trigger bounce animation
        }

    }

    public void AddEXP(int amount)
    {
        eXP += amount;
        UpdateGUI(); // Update the GUI after adding EXP
    }

    public void AddCore(int amount)
    {
        if(amount <= 0) return;

        core += amount;
        UpdateGUI(); // Update the GUI after adding core
        if(coreAnimator != null )
        {
           coreAnimator.SetTrigger("Bounce"); // Trigger bounce animation
        }
    }

    void UpdateGUI()
    {
        inkText.text = ink.ToString();
        expText.text = eXP.ToString();
        coreText.text = core.ToString();
    }
}
