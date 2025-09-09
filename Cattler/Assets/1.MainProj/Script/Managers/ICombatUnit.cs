using UnityEngine;

public interface ICombatUnit
{
    int AttackPower { get; set; }
    void TakeDamage(int amount);
    void Attack(ICombatUnit target);
    void TryAttack(Collider2D other);
}