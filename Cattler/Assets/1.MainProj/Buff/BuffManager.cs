using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    [Header("All Buff Types")]
    public Buff damageUp;
    public Buff attackSpeedUp;
    public Buff coolDownReduction;
    public Buff healthRegen;

    public GameObject iconPrefab; // contains buff icon sprite renderer, used for showing buff icon above enemy head.


    void Awake()
    {
        instance = this;
    }

    public void ApplyBuff(GameObject target , Buff buff, float duration , float strength)
    {
        Unit unit = target.GetComponent<Unit>();
        if(unit != null)
        {
            unit.AddBuff(buff, duration, strength);
            Debug.Log($"Applied {buff.buffType} to {target.name}");
        }
    }

}
