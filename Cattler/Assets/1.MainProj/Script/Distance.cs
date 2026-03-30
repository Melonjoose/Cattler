using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour

{
    public static Distance instance;
    public float distanceTraveled;
    public int dstanceCheckPoint;

    //distance text//
    public GameObject distanceTextGRP;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI mileStoneText;
    
    public Slider distanceSlider;

    //timer text//
    public GameObject timerTextGRP;
    public TextMeshProUGUI timerText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowDistance()
    {
        distanceTextGRP.SetActive(true);
        timerTextGRP.SetActive(false);
    }
    public void ShowTimer()
    {
        distanceTextGRP.SetActive(false);
        timerTextGRP.SetActive(true);
    }
    public void UpdateDistanceUI(float Distance)
    {
        distanceText.text = Distance.ToString("F2");
        distanceSlider.value = Distance;
    }

    public void UpdateMileStoneText(float mileStone)
    {
        mileStoneText.text = mileStone.ToString();
    }

    public void NoMoreMilestone() //for distance
    {
        mileStoneText.text = "\u221E";
        mileStoneText.fontSize = 25;
    }

    public void UpdateTimerText(float time)
    {
        timerText.text = time.ToString("F1") + "s";
    }
}
