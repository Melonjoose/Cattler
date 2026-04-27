using UnityEngine;
using System.Collections;

public class FirstAid : CatSkill
{
    public GameObject foodPrefab; // Prefab for the food that will be thrown
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.RandomizeTimerStart();
        foodPrefab = itemObject;
    }

    // Update is called once per frame

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("Cooking skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        Vector3 offset = transform.up * 1.5f; // Offset to spawn the food above the cat
        yield return new WaitForSeconds(0.5f);

        StatFXManager.instance.PlayVFX(transform.position, 4); // Play cooking effect at the cat's position
        GameObject NewFood = Instantiate(foodPrefab, transform.position + offset, Quaternion.identity);
        NewFood.SetActive(true);


        Vector3 ThrowDirection = Random.Range(0, 2) == 0 ? transform.right : -transform.right; // Randomly choose left or right
        int randomForce = Random.Range(2, 4); // Random force between 2 and 4
        NewFood.GetComponent<Rigidbody2D>().AddForce(ThrowDirection * randomForce + Vector3.up * randomForce * 2, ForceMode2D.Impulse);
    }
}
