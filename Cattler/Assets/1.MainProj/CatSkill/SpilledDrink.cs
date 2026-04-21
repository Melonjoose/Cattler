using UnityEngine;
using System.Collections;

public class SpilledDrink : CatSkill
{
    public int projectileCount = 3; // Number of projectiles to spawn
    public GameObject puddle; // Assign the puddle prefab in the Inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("Frosty Treat Skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        //spawns the icecream sprite to be lobbed forward at enemies. 
        yield return new WaitForSeconds(0.5f);
        //Instantiate the food gameobject and apply force to it to create the arc motion forward towards enemy. forwards as in right of screen in 2Dspace.
        Vector3 itemSpawnOffset = new Vector3(0, 1.5f, 0); // Adjust as needed

        StatFXManager.instance.PlayVFX(transform.position, 4); // Play cooking effect at the cat's position
        for (int i = 0; i < projectileCount; i++)
        {
            yield return new WaitForSeconds(0.1f); // delay between shots
            GameObject NewFood = Instantiate(itemObject, transform.position + itemSpawnOffset, Quaternion.identity);
            AudioManager.instance.PlaySFX("TeaCup"); // Play throw sound effect
            NewFood.SetActive(true);

            float randomForce = Random.Range(5, 6);

            // Add angle spread
            float angleOffset = Random.Range(-12f, 12f);
            Vector2 throwDir = Quaternion.Euler(0, 0, angleOffset) * Vector2.right;

            NewFood.GetComponent<Rigidbody2D>().AddForce(throwDir * randomForce + Vector2.up * randomForce * 2, ForceMode2D.Impulse);
        }

    }
}
