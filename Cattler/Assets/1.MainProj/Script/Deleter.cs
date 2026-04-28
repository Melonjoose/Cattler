using UnityEngine;

public class Deleter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyUnit enemyUnit = other.gameObject.GetComponent<EnemyUnit>();
            if(enemyUnit != null)
            {
                enemyUnit.TakeDamage(null,99999,0);
            }
        }

    }
}
