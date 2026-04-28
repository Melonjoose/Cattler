using UnityEngine;
using System.Collections;

public class MarionetteString : MonoBehaviour
{
    public PuppetDance puppetDance;
    public EnemyUnit enemyUnit; //enemy this item is attached to
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyUnit = GetComponentInParent<EnemyUnit>(); // since this object is attached inside of the enemy
        StartCoroutine(StringSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StringSequence()
    {
        yield return new WaitForSeconds(0.5f);
        if (enemyUnit != null)
        {
            AudioManager.instance.PlaySFX("MarionetteString");
            int randomKnockabackPower = Random.Range(7, 10);
            enemyUnit.TakeDamage(puppetDance.catUnit, 0, randomKnockabackPower); //takes no damage but knocks them backwards
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
