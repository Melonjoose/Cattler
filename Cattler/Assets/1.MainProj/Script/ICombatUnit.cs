public interface ICombatUnit
{
    void TakeDamage(int amount);
    void Attack(ICombatUnit target);
}