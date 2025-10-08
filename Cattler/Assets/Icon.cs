using UnityEngine;

public class Icon : MonoBehaviour
{
    public CatMovement linkedCat;

    public int iconIndex = 0; // Position in SlidingIcons list
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetIndex() => iconIndex;

    public void AssignIconIndex(int catIndex)
    {
        iconIndex = catIndex; // Sync icon index with cat position index
    }


}
