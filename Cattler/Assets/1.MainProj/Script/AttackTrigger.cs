using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private ICombatUnit parentUnit;

    private void Start()
    {
        parentUnit = GetComponentInParent<ICombatUnit>();
        if (parentUnit == null)
            Debug.LogError("Parent does not implement ICombatUnit interface!");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (parentUnit != null)
        {
            parentUnit.TryAttack(other);
        }
    }
}