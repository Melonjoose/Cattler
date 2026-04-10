using UnityEngine;
using System.Collections.Generic;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager instance;

    [Header("All Debuff Types")]
    public Debuff stun;
    public Debuff slow;
    public Debuff dot; // damage over time, burn,poison etc.

    public GameObject iconPrefab; // contains debuff icon sprite renderer, used for showing debuff icon above enemy head.

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
               ApplyDebuff(enemy,stun);
            }
        }
    }

    public void ApplyDebuff(GameObject target, Debuff debuff) // to apply debuff to the target.
    {
        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {
            unit.AddDebuff(debuff);
            Debug.Log($"Applied {debuff.debuffType} to {target.name}");
        }
    }

    public void RemoveDebuff(GameObject target, Debuff debuff)
    {
        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {
            unit.OnDebuffRemoved(debuff);
        }
    }

}