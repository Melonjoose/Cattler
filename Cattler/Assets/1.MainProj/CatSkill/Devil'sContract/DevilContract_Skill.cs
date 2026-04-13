using System.Collections;
using UnityEngine;

public class DevilContract : CatSkill
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //pick a random ally cat unit,Deal damage to a friendly cat to instant kill one non-boss enemy
    public GameObject curse; //this appears on the cat that is cursed.
    public GameObject killMark;  //this appears on enemy marked for death.

    void Start()
    {
        catUnit = GetComponentInParent<CatUnit>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkill()
    {
        // Implement the logic for Blinding Light skill here
        StartCoroutine(DevilContractSequence());
        //AudioManager.instance.PlaySFX("BlindingLight"); // Play the blinding light sound effect (make sure to have this sound in your AudioManager)
    }

    IEnumerator DevilContractSequence()
    {
        CatUnit targetCat = ChooseRandomCat();
        if (targetCat != null && targetCat != catUnit)
        {
            // Apply curse effect to the chosen cat
            GameObject curseEffect = Instantiate(curse, targetCat.transform);
            curseEffect.transform.localPosition = Vector3.zero;

            if (targetCat.runtimeData.currentHealth > 1 && targetCat != catUnit)
            {
                targetCat.TakeDamage(1); // take some damage
                yield return new WaitForSeconds(1f);

                EnemyUnit targetEnemy = ChooseRandomEnemy();
                if (targetEnemy != null)
                {
                    GameObject killMarkEffect = Instantiate(killMark, targetEnemy.transform);
                    killMarkEffect.transform.localPosition = Vector3.zero;

                    targetEnemy.TakeDamage(targetEnemy.currentHealth);
                    Destroy(killMarkEffect, 1f);
                }
            }

            // Always clean up curse effect, even if caster was chosen
            Destroy(curseEffect, 1f);
        }
    }

    public CatUnit ChooseRandomCat()
    {
               CatUnit[] cats = TeamManager.instance.cats.ToArray(); //look at team manager list of cats and pick one randomly.
        if (cats.Length == 0)
            return null;
        int randomIndex = Random.Range(0, cats.Length);
        return cats[randomIndex];
    }

    public EnemyUnit ChooseRandomEnemy()
    {
        //later add a check to ensure that the enemy chosen is not a boss.
        EnemyUnit[] enemies = FindObjectsOfType<EnemyUnit>();
        if (enemies.Length == 0)
            return null;
        int randomIndex = Random.Range(0, enemies.Length);
        return enemies[randomIndex];
    }
}
