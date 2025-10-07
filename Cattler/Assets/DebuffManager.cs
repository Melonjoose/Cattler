using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Stunned(GameObject target, float duration)
    {
        if (target.GetComponent<CatUnit>() != null)
        {
            target.GetComponent<CatUnit>().Stunned(duration);
        }
        else if (target.GetComponent<EnemyUnit>() != null)
        {
            //target.GetComponent<EnemyUnit>().Stunned(duration);
        }
    }
    public void RemoveStun(GameObject target)
    {
        // Example if you store stuns in a dictionary or flag
        CatUnit cat = target.GetComponent<CatUnit>();
        if (cat != null)
        {
            cat.Unstun();
            // Optionally stop any coroutines or reset speed here
        }
    }
}
