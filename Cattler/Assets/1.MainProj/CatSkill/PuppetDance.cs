using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetDance : CatSkill
{
    public Vector3 offset = new Vector3(0, 2, 0);
    public GameObject marionettePrefab;
    public GameObject mainMarionette;
    protected override void Start()
    {
        base.Start();
        mainMarionette.gameObject.SetActive(false);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("PuppetDance skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        yield return new WaitForSeconds(0.5f);
        mainMarionette.SetActive(true); //visuals for skill activation
        // Find all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Debug.Log("No enemies found for PuppetDance.");
            yield break;
        }

        // Shuffle or pick up to 3 random enemies
        int count = Mathf.Min(3, enemies.Length);
        List<GameObject> chosen = new List<GameObject>();
        while (chosen.Count < count)
        {
            GameObject candidate = enemies[Random.Range(0, enemies.Length)];
            if (!chosen.Contains(candidate))    
                chosen.Add(candidate);
        }

        // Instantiate marionette inside each chosen enemy
        foreach (GameObject enemy in chosen)
        {
            Transform enemyTransform = enemy.transform;
            GameObject marionetteObject = Instantiate(marionettePrefab, enemyTransform.position + offset, Quaternion.identity, enemyTransform);
            marionetteObject.SetActive(true);

            EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();

            MarionetteString marionetteCS = marionetteObject.GetComponent<MarionetteString>();
            marionetteCS.enemyUnit = enemyUnit;
            marionetteCS.puppetDance = this;
        }
        yield return new WaitForSeconds(2f);
        mainMarionette.SetActive(false); //visuals for skill activation
    }

}