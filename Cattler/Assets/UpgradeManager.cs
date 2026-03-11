using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //in-charge of upgrades.
    public static UpgradeManager instance;

    [SerializeField]
    // Name Of Upgrade
    // Times it has been upgraded (value int.)
    // Price to upgrade



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
