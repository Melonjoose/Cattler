using System.Collections.Generic;
using UnityEngine;

public class StatFXManager : MonoBehaviour
{
    public static StatFXManager instance;

    [Header("Stat FX Pool")]
    public GameObject SFXPoolContainer;
    public int StatSFXpoolSize = 5;
    public StatFX SFXPrefab;
    private Queue<StatFX> pool = new Queue<StatFX>();

    [Header("On-Hit VFX Pool")]
    public GameObject VFXPoolContainer;
    public int onHitFXPoolSize = 5;
    public List<GameObject> onHitFXPrefabs = new List<GameObject>();

    // Dictionary: prefabIndex > queue of pooled objects
    private Dictionary<int, Queue<GameObject>> vfxPools = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        instance = this;

        // Initialize StatFX pool
        for (int i = 0; i < StatSFXpoolSize; i++)
        {
            StatFX sfx = Instantiate(SFXPrefab, SFXPoolContainer.transform);
            sfx.gameObject.SetActive(false);
            pool.Enqueue(sfx);
        }

        // Initialize pools for each prefab type
        for (int i = 0; i < onHitFXPrefabs.Count; i++)
        {
            vfxPools[i] = new Queue<GameObject>();
            for (int j = 0; j < onHitFXPoolSize; j++)
            {
                GameObject vfx = Instantiate(onHitFXPrefabs[i], VFXPoolContainer.transform);
                vfx.SetActive(false);
                vfxPools[i].Enqueue(vfx);
            }
        }
    }

    // --- StatFX Pool ---
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
        sfx.transform.SetParent(SFXPoolContainer.transform);
        sfx.gameObject.SetActive(false);
        pool.Enqueue(sfx);
    }

    // --- VFX Pool ---
    public GameObject GetVFXFromPool(int prefabIndex)
    {
        if (!vfxPools.ContainsKey(prefabIndex)) return null;

        GameObject vfx;
        if (vfxPools[prefabIndex].Count > 0)
        {
            vfx = vfxPools[prefabIndex].Dequeue();
        }
        else
        {
            vfx = Instantiate(onHitFXPrefabs[prefabIndex], VFXPoolContainer.transform);
        }

        // Assign prefabIndex to the helper script
        VFXreturner returner = vfx.GetComponent<VFXreturner>();
        if (returner != null)
            returner.prefabIndex = prefabIndex;

        vfx.SetActive(true);
        return vfx;
    }


    public void ReturnVFXToPool(GameObject vfx, int prefabIndex)
    {
        vfx.transform.SetParent(VFXPoolContainer.transform);
        vfx.SetActive(false);
        vfxPools[prefabIndex].Enqueue(vfx);
    }

    public void PlayVFX(Vector3 position, int prefabIndex = 0)
    {
        if (prefabIndex < 0 || prefabIndex >= onHitFXPrefabs.Count) return;

        GameObject vfx = GetVFXFromPool(prefabIndex);
        vfx.transform.position = position;

        ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            float duration = ps.main.duration + ps.main.startLifetime.constantMax;
            StartCoroutine(ReturnAfterDuration(vfx, prefabIndex, duration));
        }
    }

    private System.Collections.IEnumerator ReturnAfterDuration(GameObject vfx, int prefabIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnVFXToPool(vfx, prefabIndex);
    }

    public void ReturnAfterAnimation(GameObject vfx, int prefabIndex)
    {
        ReturnVFXToPool(vfx, prefabIndex);
    }

}