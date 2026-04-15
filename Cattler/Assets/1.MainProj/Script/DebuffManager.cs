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
               ApplyDebuff(enemy,stun, 5f);
            }
        }
    }

    public void ApplyDebuff(GameObject target, Debuff debuff , float duration) // to apply debuff to the target.
    {
        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {
            unit.AddDebuff(debuff, duration);
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

    public void ClearAllDebuff(GameObject target)
    {
        // This method can be called when the unit dies or when a debuff is cleansed.
        // It should remove all debuffs from the unit and clear any associated UI elements.
        Unit unit = target.GetComponent<Unit>();
        if (unit!=null)
        {
            unit.RemoveAllDebuffs();
        }
    }
}