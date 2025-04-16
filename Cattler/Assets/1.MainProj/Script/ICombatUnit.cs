using UnityEngine;

public interface ICombatUnit
{
    void TakeDamage(int amount);
    void Attack(ICombatUnit target);
    void TryAttack(Collider2D other);
}