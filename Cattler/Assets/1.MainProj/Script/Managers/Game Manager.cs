using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Pages")]
    [SerializeField] private List<GameObject> Pages = new List<GameObject>();

     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenSummonPage()
    {
        // Logic to open the summon page
        Debug.Log("Opening Summon Page");
        // You can implement UI opening logic here
    }

    public void CloseSummonPage()
    {
        // Logic to close the summon page
        Debug.Log("Closing Summon Page");
        // You can implement UI closing logic here
    }
}
