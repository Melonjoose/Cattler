using TMPro;
using UnityEngine;

public class RetreatPage : MonoBehaviour
{
    public TextMeshProUGUI inkEarned; //manually referenced
    public TextMeshProUGUI coreEarned;
    public TextMeshProUGUI itemsEarned;
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
        inkEarned.text = "Ink Earned: " + Currency.instance.inkEarnedThisMission.ToString();
        coreEarned.text = "Core Earned: " + Currency.instance.coreEarnedThisMission.ToString();
        itemsEarned.text = "Items Earned: " + Currency.instance.itemsEarnedThisMission.ToString();
    }

    public void OpenRetreatPage() 
    {
        UpdateUI();
    }
}
