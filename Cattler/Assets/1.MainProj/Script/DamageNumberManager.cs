using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance;

    private Transform canvasTransform;

    private void Awake()
    {
        Instance = this;
        canvasTransform = GameObject.Find("Canvas").transform;
    }

    private void Start()
    {
        Instance = this;
    }

    public void ShowDamage(int amount, Vector3 worldPosition)
    {
        DamageNumber dn = DamageNumberPool.Instance.GetFromPool();

        Vector3 spawnPos = worldPosition + Vector3.up * 1f;
        dn.transform.position = spawnPos;

        //Debug.Log($"[ShowDamage] Spawning damage number at worldPosition: {worldPosition}, final spawnPos: {spawnPos}");

        dn.SetText(amount);
    }
}
