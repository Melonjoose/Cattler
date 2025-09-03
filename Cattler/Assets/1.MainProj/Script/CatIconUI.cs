using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class CatIconUI : MonoBehaviour
{
    public static CatIconUI instance;
    [Header("CatIcon1")]
    public bool catIcon1Active;
    public bool catIcon2Active;
    public bool catIcon3Active;
    public bool catIcon4Active;
    public bool catIcon5Active;
    public Image catIcon1;
    public Image catIcon2;
    public Image catIcon3;
    public Image catIcon4;
    public Image catIcon5;
    public Slider catIconHealthBar1;
    public Slider catIconHealthBar2;
    public Slider catIconHealthBar3;
    public Slider catIconHealthBar4;
    public Slider catIconHealthBar5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ActivateHealthBar();
    }

    void ActivateHealthBar()
    {
        if(catIcon1Active){catIconHealthBar1.gameObject.SetActive(true);} else{catIconHealthBar1.gameObject.SetActive(false);}
        if(catIcon2Active){catIconHealthBar2.gameObject.SetActive(true);} else{catIconHealthBar2.gameObject.SetActive(false);}
        if(catIcon3Active){catIconHealthBar3.gameObject.SetActive(true);} else{catIconHealthBar3.gameObject.SetActive(false);}
        if(catIcon4Active){catIconHealthBar4.gameObject.SetActive(true);} else{catIconHealthBar4.gameObject.SetActive(false);}
        if(catIcon5Active){catIconHealthBar5.gameObject.SetActive(true);} else{catIconHealthBar5.gameObject.SetActive(false);}
    }
}
