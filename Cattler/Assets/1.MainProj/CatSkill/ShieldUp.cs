using UnityEngine;

public class ShieldUp : CatSkill
{
    public Transform shieldPosition; // Position where the shield will be created
    public Shield shield;
    public int shieldHealth = 5; // Health of the shield
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemObject.SetActive(false); // Ensure the shield is initially inactive
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseSkill()
    {
        base.UseSkill();
        CreateShield();
        //AudioManager.instance.PlaySFX("ShieldUp"); // Play the shield up sound effect (make sure to have this sound in your AudioManager)
    }

    public void CreateShield()
    {
        //creates a shield by activating itemGameobject.
        //when active , reset shield health to 5.
        //when enemy collide with shield, enemy take 1 damage and take mnassive knockback,
        //when shield drops to 0, deactivate shield and start cooldown.
        itemObject.SetActive(true);
        shieldHealth = (int)value; // Set shield health to the value defined in the skill (e.g., 5)
        isActive = false; // stop timer from running when shield is active
    }

    public void DestroyShield()
    {
        //deactivate shield and start cooldown.
        //play animation before shield is set false.
        itemObject.SetActive(false);
        isActive = true; // Start cooldown timer when the shield is destroyed
    }

    //when shield is active and collide with an enemy, shield minus one, and enemy takes damage and knockback.//play animation when animation is collided.
}
