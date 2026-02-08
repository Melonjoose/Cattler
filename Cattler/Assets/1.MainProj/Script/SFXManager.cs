using System.Collections.Generic;
using UnityEngine;

public class StatFXManager : MonoBehaviour
{
    public static StatFXManager instance;
    public GameObject SFXPoolContainer;
    public int poolSize = 30;

    public StatFX SFXPrefab;

    private Queue<StatFX> pool = new Queue<StatFX>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            StatFX sfx = Instantiate(SFXPrefab, SFXPoolContainer.transform);
            sfx.gameObject.SetActive(false);
            pool.Enqueue(sfx);
        }
    }

    public StatFX GetFromPool()
    {
        if (pool.Count > 0)
        {
            StatFX sfx = pool.Dequeue();
            sfx.gameObject.SetActive(true);
            return sfx;
        }
        else
        {
            StatFX sfx = Instantiate(SFXPrefab, SFXPoolContainer.transform);
            return sfx;
        }
    }

    public void ReturnToPool(StatFX sfx)
    {
        sfx.transform.SetParent(SFXPoolContainer.transform); // Re-parent under DamageNumberPool
        sfx.gameObject.SetActive(false);
        pool.Enqueue(sfx);
    }

    public void ShowCrystalFX()
    {

    }

}
