using UnityEngine;
using System.Collections;

public class TeaParty : ActiveAbility
{
    public int minSpawn = 5;
    public int maxSpawn = 8;
    public float spacing = 1.0f;        // horizontal spacing between tea
    public float spawnDelay = 0.1f;     // delay between each spawn
    public GameObject teaPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LifetimeSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LifetimeSequence()
    {

        Vector3 startPos = transform.position + spawnLocationOffset;

        int teaCount = Random.Range(minSpawn, maxSpawn);

        for (int i = 0; i < teaCount; i++)
        {
            Vector3 spawnPos = startPos + new Vector3(0, i * spacing, 0);

            GameObject Tea = Instantiate(teaPrefab, spawnPos, Quaternion.Euler(0, 0, 35));
            Tea.SetActive(true);
            Vector3 ThrowDirection = Random.Range(0, 2) == 0 ? transform.right : -transform.right; // Randomly choose left or right
            int randomForce = Random.Range(2, 4); // Random force between 2 and 4
            Tea.GetComponent<Rigidbody2D>().AddForce(ThrowDirection * randomForce + Vector3.up * randomForce * 2, ForceMode2D.Impulse);
            // Do NOT parent to FireCarpet, let them live independently
            // flame.transform.SetParent(transform);

            yield return new WaitForSeconds(spawnDelay);
            Debug.Log("Spawned Tea #" + i);
        }

        // After spawning all flames, destroy this ability object
        Destroy(gameObject);
    }
}
