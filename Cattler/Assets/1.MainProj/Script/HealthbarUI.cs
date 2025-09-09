using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    public Slider[] healthBarUI;
    public Healthbar healthbar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthbar(healthbar);
    }

    public void SetHealthBarUI()
    {
        //foreach loop to scan through list in SlidingIcon.cs and assign Slider located as a child of the Iconlist to healthBarUI
    }

    public void SetHealthbar(Healthbar hb)
    {
        //assign healthbar 
    }


}
