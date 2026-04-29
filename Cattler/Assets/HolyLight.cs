using UnityEngine;
using System.Collections;

public class HolyLight : ActiveAbility
{
    public LightAura aura;
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
        aura.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
