using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour

{
    public static Distance instance;
    public float distanceTraveled;
    public int dstanceCheckPoint;
    public TextMeshProUGUI distanceText;
    public Slider distanceSlider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateDistanceUI(float Distance)
    {
        distanceText.text = Distance.ToString("F0");
        distanceSlider.value = Distance;
    }
}
