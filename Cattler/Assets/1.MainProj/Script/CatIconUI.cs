using UnityEngine;
using UnityEngine.UI;

public class CatIconUI : MonoBehaviour
{
    public CatIconUI instance;
    [Header("CatIcon1")]
    public bool catIcon1Active;
    public Image catIcon1;
    public Slider catIconHealthBar1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
