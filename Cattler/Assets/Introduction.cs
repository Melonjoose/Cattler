using UnityEngine;

public class Introduction : MonoBehaviour
{
    public static Introduction instance;
    public Animator animator;
    public Canvas introCanvas;
    public bool inIntroduction = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (inIntroduction)
        {
            introCanvas.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayButtonClick()
    {
        AudioManager.instance.PlaySFX("Button1");
    }

    void PlayMeow()
    {
        AudioManager.instance.PlaySFX("Meow");
    }

    private bool screenClicked = false;
    public void ClickOnScreen()
    {
        if(screenClicked) {return; }
        screenClicked = true;
        animator.SetTrigger("Click");
    }

    void ExitIntroduction()
    {
        //open Transition.SlowFade
        Transition.instance.FadeInSlow();
        Tutorial.instance.OpeningScene();
        AudioManager.instance.TransitionTheme("Lobby");
    }
    
    void CloseCanvas()
    {
        //close Introduction
        introCanvas.gameObject.SetActive(false);
    }
}
