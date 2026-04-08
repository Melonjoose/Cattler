using UnityEngine;
using System.Collections.Generic;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager instance;

    [Header("All Debuff Types")]
    public Debuff stun;
    public Debuff slow;
    public Debuff dot; // damage over time, burn,poison etc.

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
                DebuffManager.instance.ApplyDebuff(enemy, DebuffManager.instance.stun);
            }
        }
    }

    public void ApplyDebuff(GameObject target, Debuff debuff) // to apply debuff to the target.
    {
        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {
            unit.OnDebuffApplied(debuff);
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