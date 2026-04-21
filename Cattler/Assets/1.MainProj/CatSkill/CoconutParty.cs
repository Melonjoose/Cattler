using UnityEngine;
using System.Collections;

public class CoconutParty : CatSkill
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("Cooking skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        //chef skill to cook food, then throws the cook food infront of the cat in an arc motion. 
        //Animation or effect plays.
        yield return new WaitForSeconds(0.5f);
        //Instantiate the food gameobject and apply force to it to create the arc motion
        Vector3 itemSpawnOffset = new Vector3(0,1.5f,0); // Adjust as needed

        StatFXManager.instance.PlayVFX(transform.position, 4); // Play cooking effect at the cat's position
        GameObject NewFood = Instantiate(itemObject, transform.position + itemSpawnOffset, Quaternion.identity);
        NewFood.SetActive(true);
        //the food will have it's own script to handle movement and picking up by players.
        //Throw food in a arc motion randomly.
        Vector3 ThrowDirection = Random.Range(0, 2) == 0 ? transform.right : -transform.right; // Randomly choose left or right
        int randomForce = Random.Range(2, 4); // Random force between 2 and 4
        NewFood.GetComponent<Rigidbody2D>().AddForce(ThrowDirection * randomForce + Vector3.up * randomForce * 2, ForceMode2D.Impulse);
    }
}
