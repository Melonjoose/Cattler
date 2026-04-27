using UnityEngine;
using System.Collections;

public class DangerousSpeech : CatSkill
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        itemObject.gameObject.SetActive(false);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("DangerousSpeech skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        yield return new WaitForSeconds(0.5f);
        Transform enemy = RandomEnemy();
        if(enemy != null)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), Random.Range(1f, 5f), 0f);
            GameObject word = Instantiate(itemObject, spawnPos, Quaternion.identity);
            word.gameObject.SetActive(true);
            word.transform.localScale = Vector3.zero;
            WordProjectile wordCS = word.GetComponent<WordProjectile>();
            wordCS.dangerousSpeech = this;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                word.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }

        }
        else
        {
            Debug.Log("there are no enemies");
        }
    }
    public Transform RandomEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        int randomIndex = Random.Range(0, enemies.Length);
        return enemies[randomIndex].transform;
    }
}
