using System.Collections;
using UnityEngine;

public class HeavyShot : ActiveAbility
{
    public GameObject bulletsprite;
    public Collider2D bulletCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletsprite = transform.GetChild(0).gameObject;
        bulletCollider = bulletsprite.GetComponent<Collider2D>();
        StartCoroutine(Spritemovement());
    }

    IEnumerator Spritemovement()
    {
        Vector3 leftStart = bulletsprite.transform.position;
        Vector3 leftEnd = leftStart + new Vector3(+1, 0, 0)* movespeed;

        float duration = lifetime; // seconds
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            bulletsprite.transform.position = Vector3.Lerp(leftStart, leftEnd, t);

            yield return null; // wait until next frame
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemy = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemy != null && catUnit != null)
        {
            int damage = Mathf.CeilToInt(catUnit.runtimeData.attackPower * damageMultiplier);
            enemy.TakeDamage(damage);
            DamageNumberManager.Instance.ShowDamage((int)damage, enemy.transform.position);
            //Debug.Log("enemy hit by slash, taking" + damage);
        }
    }
    //reference the 2 child sprite objects and collider
    //animate the objects by moving them in a cross pattern.
    //in collision with enemy, deal damage.
    //destroy the dagger cross after the animation is complete or after a certain time.
}
