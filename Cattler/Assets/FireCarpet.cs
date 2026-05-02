using UnityEngine;
using System.Collections;

public class FireCarpet : ActiveAbility
{
    public GameObject fireLinePrefab;
    public int knockback = 5;
    public int fireCount = 8;           // number of flames in the line
    public float spacing = 1.0f;        // horizontal spacing between flames
    public float spawnDelay = 0.1f;     // delay between each spawn
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CarpetSequence());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CarpetSequence()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 startPos = transform.position;

        for (int i = 0; i < fireCount; i++)
        {
            Vector3 spawnPos = startPos + new Vector3(i * spacing, 0, 0);

            GameObject flame = Instantiate(fireLinePrefab, spawnPos, Quaternion.Euler(0, 0, 35));
            AudioManager.instance.PlaySFX(skillSound);
            FireLine fireLine = GetComponent<FireLine>();
            if (fireLine != null)
            {
                fireLine.damage = damage;
                fireLine.knockback = knockback;
            }
            flame.SetActive(true);

            // Do NOT parent to FireCarpet, let them live independently
            // flame.transform.SetParent(transform);

            yield return new WaitForSeconds(spawnDelay);
            Debug.Log("Spawned flame #" + i);
        }

        // After spawning all flames, destroy this ability object
        Destroy(gameObject);
    }

}
