using UnityEngine;

public class Currency : MonoBehaviour
{

    public int ink = 0; // Ink currency drop by all enemies. Bosses drop more ink than enemies.
    public int eXP = 0; // EXP can be earn after the player retreats. EXP can also be earn from dead cats
    public int core = 0; //Rare currency drop by mini bosses and bosses
    public static Currency instance; //Singleton instance to access this script easily from other scripts

    //might want to make this script accessible to all other scripts

    void Start()
    {
        instance = this;
        //Check for save data of currency
    }

    public void AddInk(int amount)
    {
        ink += amount;
    }

    public void AddEXP(int amount)
    {
        eXP += amount;
    }

    public void AddCore(int amount)
    {
        core += amount;
    }
}
