using UnityEngine;
using System.Collections.Generic;

public class DamageNumberPool : MonoBehaviour
{
    public static DamageNumberPool Instance;
    public DamageNumber damageNumberPrefab;
    public int poolSize = 50;

    private Queue<DamageNumber> pool = new Queue<DamageNumber>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            DamageNumber dn = Instantiate(damageNumberPrefab, transform);
            dn.gameObject.SetActive(false);
            pool.Enqueue(dn);
        }
    }

    public DamageNumber GetFromPool()
    {
        if (pool.Count > 0)
        {
            DamageNumber dn = pool.Dequeue();
            dn.gameObject.SetActive(true);
            return dn;
        }
        else
        {
            DamageNumber dn = Instantiate(damageNumberPrefab, transform);
            return dn;
        }
    }

    public void ReturnToPool(DamageNumber dn)
    {
        dn.transform.SetParent(transform); // Re-parent under DamageNumberPool
        dn.gameObject.SetActive(false);
        pool.Enqueue(dn);
    }



}
