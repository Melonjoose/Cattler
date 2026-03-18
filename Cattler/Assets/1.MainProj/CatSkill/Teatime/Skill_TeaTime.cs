using System.Collections;
using UnityEngine;

public class Skill_TeaTime : CatSkill
{
    public Vector3 skillLocation; // The location where the skill will be activated (e.g., in front of the cat)
    public GameObject drinkPrefab; // Prefab for the food that will be thrown
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drinkPrefab = itemObject;
        skillLocation = transform.position + (transform.up * 1.5f); // above the cat. 
        this.gameObject.transform.position = skillLocation; // Set the skill's game object to the location where it will be activated
    }

    // Update is called once per frame
    void FixedUpdate() // Use FixedUpdate for consistent timing, especially if the skill's effects are time-sensitive
    {
        Cooldown();
    }

    public void Cooldown()
    {
        if (isActive == false)
        {
            return;
        }
        if(time <= 0)//check if the cooldown time has elapsed
        {
            UseSkill();
            time = cooldown; //reset time to match the cooldown duration
        }

        time -= Time.deltaTime; //decrease time by the time that has passed since the last frame
    }

    public void UseSkill()
    {
        // Implement the logic for the cooking skill here
        Debug.Log("Teatime Skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        //chef skill to cook food, then throws the cook food infront of the cat in an arc motion. 
        //Animation or effect plays.
        yield return new WaitForSeconds(0.5f);
        //Instantiate the food gameobject and apply force to it to create the arc motion

        StatFXManager.instance.PlayVFX(transform.position, 4); // Play cooking effect at the cat's position
        GameObject NewFood = Instantiate(drinkPrefab, transform.position + transform.forward * 1.5f, Quaternion.identity);
        NewFood.SetActive(true);
        //the food will have it's own script to handle movement and picking up by players.
        //Throw food in a arc motion randomly.
        Vector3 ThrowDirection = Random.Range(0, 2) == 0 ? transform.right : -transform.right; // Randomly choose left or right
        int randomForce = Random.Range(2, 4); // Random force between 2 and 4
        NewFood.GetComponent<Rigidbody2D>().AddForce(ThrowDirection*randomForce + Vector3.up * randomForce*2,ForceMode2D.Impulse);
    }
}
