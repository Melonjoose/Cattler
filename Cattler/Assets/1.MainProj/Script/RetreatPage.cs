using TMPro;
using UnityEngine;

public class RetreatPage : MonoBehaviour
{
    public TextMeshProUGUI inkEarned; //manually referenced
    public TextMeshProUGUI coreEarned;
    public TextMeshProUGUI itemsEarned;
    public TextMeshProUGUI travelDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    // Update is called once per frame
    private void FixedUpdate()
    {

    }

    void UpdateUI()
    {
        inkEarned.text = Currency.instance.inkEarnedThisMission.ToString();
        coreEarned.text = Currency.instance.coreEarnedThisMission.ToString();
        itemsEarned.text = Currency.instance.itemsEarnedThisMission.ToString();
        string distance = TravelManager.instance.distanceTraveledUIvalue.ToString("F2");
        travelDistance.text = $"You've travelled {distance} km";
    }

    public void OpenRetreatPage() 
    {
        UpdateUI();
    }
}
