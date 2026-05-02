using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator transitionAnimator;
    public static Transition instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        FadeInSlow();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeIn() //fade to black
    {
        transitionAnimator.SetTrigger("FadeIn");
    }

    public void FadeInSlow() //fade to black
    {
        transitionAnimator.SetTrigger("FadeInSlow");
    }

    public void FadeOut()
    {
        transitionAnimator.SetTrigger("FadeOut");
    }

    public void FadeOutSlow()
    {
        transitionAnimator.SetTrigger("FadeOutSlow");
    }

}
