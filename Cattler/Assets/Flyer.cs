using UnityEngine;

public class FlyerUnit : EnemyUnit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.OnUnitStart();
    }

    // Update is called once per frame
    void Update()
    {
        base.OnUnitUpdate();
    }
    protected override void OnUnitStart()
    {

    }
}
