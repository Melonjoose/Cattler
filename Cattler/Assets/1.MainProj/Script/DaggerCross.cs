using System.Collections;
using UnityEngine;

public class DaggerCross : ActiveAbility
{
    public GameObject leftsprite;
    public GameObject rightsprite;
    public Collider2D leftCollider;
    public Collider2D rightCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftsprite = transform.GetChild(0).gameObject;
        rightsprite = transform.GetChild(1).gameObject;
        leftCollider = leftsprite.GetComponent<Collider2D>();
        rightCollider = rightsprite.GetComponent<Collider2D>();
        StartCoroutine(Spritemovement());
    }

    IEnumerator Spritemovement()
    {
        Vector3 leftStart = leftsprite.transform.position;
        Vector3 leftEnd = leftStart + new Vector3(+1, -1, 0)* movespeed;

        Vector3 rightStart = rightsprite.transform.position;
        Vector3 rightEnd = rightStart + new Vector3(-1, -1, 0)*movespeed;

        float duration = lifetime; // seconds
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            leftsprite.transform.position = Vector3.Lerp(leftStart, leftEnd, t);
            rightsprite.transform.position = Vector3.Lerp(rightStart, rightEnd, t);

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
            enemy.TakeDamage(catUnit, damage, 3f);
            DamageNumberManager.Instance.ShowDamage((int)damage, enemy.transform.position);
            //Debug.Log("enemy hit by slash, taking" + damage);
        }
    }
    //reference the 2 child sprite objects and collider
    //animate the objects by moving them in a cross pattern.
    //in collision with enemy, deal damage.
    //destroy the dagger cross after the animation is complete or after a certain time.
}
