using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public static PreviewManager instance;
    public CatUnit catUnit;
    public Item weapon1;
    public Item weapon2;
    public Item hat;
    // this is to control items to be added onto the catUnit.
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
