using UnityEngine;
using System.Collections;

public class BlackHole : CatSkill
{
    public Transform spawnLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("Blackhole skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        AudioManager.instance.PlaySFX("CastBlackMagic");
        yield return new WaitForSeconds(0.5f);
        GameObject blackHole = Instantiate(itemObject, spawnLocation.position, Quaternion.identity);
        blackHole.gameObject.SetActive(true);
    }
}
