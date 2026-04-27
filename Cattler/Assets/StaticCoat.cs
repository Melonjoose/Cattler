using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticCoat : CatSkill
{
    public Vector3 offset = new(0, -1, 0);
    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("StaticCoat skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        // fur coat glow or some visual effect
        // AudioManager.instance.PlaySFX("Lightning");
        yield return new WaitForSeconds(0.5f);

        Transform enemy = RandomEnemy();
        if (enemy != null)
        {
            GameObject lightning = Instantiate(itemObject, (enemy.position + offset), Quaternion.identity);
            Lightning lightningCS = lightning.GetComponent<Lightning>();
            lightningCS.staticCoat = this;
            lightning.SetActive(true);
            
            AudioManager.instance.PlaySFX("Lightning");
        }
        else
        {
            Debug.Log("No enemies found!");
        }
    }

    // Find a random enemy in the scene
    public Transform RandomEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        int randomIndex = Random.Range(0, enemies.Length);
        return enemies[randomIndex].transform;
    }
}
