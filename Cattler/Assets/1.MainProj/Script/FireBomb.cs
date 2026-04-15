using System.Collections;
using UnityEngine;

public class FireBomb : ActiveAbility
{
    public GameObject bombsprite;
    public GameObject carpetsprite;
    public Collider2D bombCollider;
    public Collider2D carpetCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bombsprite = transform.GetChild(0).gameObject;
        carpetsprite = transform.GetChild(1).gameObject;
        bombCollider = bombsprite.GetComponent<Collider2D>();
        carpetCollider = carpetsprite.GetComponent<Collider2D>();

        StartCoroutine(SpriteSequence());
    }

    IEnumerator SpriteSequence() 
    {
        //when bomb is instantiated.(handled by Use_skill.cs)
        //bomb sprite appears and start falling down from the sky. carpet is invisible. // I will move this.gameobject position that holds the bomb and carpet sprites.
        bombsprite.SetActive(true);
        carpetsprite.SetActive(false);

        float duration = lifetime; // seconds
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            this.transform.position += new Vector3(0, -1, 0) * movespeed * Time.deltaTime; // move downwards
            yield return null; // wait until next frame
        }
        //after bomb comes into contact with the ground, bomb sprite disappears and carpet sprite appears. // I will disable the bomb sprite and enable the carpet sprite.
        bombsprite.SetActive(false);
        carpetsprite.SetActive(true);

        //carpet sprite will scale horizontally, X-axis scales up. for a certain duration. // I will use a coroutine to scale the carpet sprite over time.
        float duration2 = lifetime; // seconds
        float elapsed2 = 0f;
        while (elapsed2 < duration2) // keep the carpet for another duration
        {
            elapsed2 += Time.deltaTime;
            carpetsprite.transform.localScale = new Vector3(10 + (elapsed2 / duration2)*range, 5, 10); // scale horizontally over time
            yield return null; // wait until next frame
        }
        //after the animation is complete or after a certain time, the carpet sprite disappears and the fire bomb object is destroyed. // I will disable the carpet sprite and destroy this.gameobject.
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
}
