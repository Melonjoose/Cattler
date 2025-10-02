using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public GameObject SFXPoolContainer;
    public int poolSize = 30;

    public SFX SFXPrefab;

    private Queue<SFX> pool = new Queue<SFX>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            SFX sfx = Instantiate(SFXPrefab, SFXPoolContainer.transform);
            sfx.gameObject.SetActive(false);
            pool.Enqueue(sfx);
        }
    }

    public SFX GetFromPool()
    {
        if (pool.Count > 0)
        {
            SFX sfx = pool.Dequeue();
            sfx.gameObject.SetActive(true);
            return sfx;
        }
        else
        {
            SFX sfx = Instantiate(SFXPrefab, SFXPoolContainer.transform);
            return sfx;
        }
    }

    public void ReturnToPool(SFX sfx)
    {
        sfx.transform.SetParent(SFXPoolContainer.transform); // Re-parent under DamageNumberPool
        sfx.gameObject.SetActive(false);
        pool.Enqueue(sfx);
    }



}
