using UnityEngine;

public class Shield : MonoBehaviour
{
    public ShieldUp shieldUp;
    public CatUnit catUnit;
    public bool animationPlaying = false; // Flag to track if the animation is currently playing
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shieldUp = GetComponentInParent<ShieldUp>(); // Get the ShieldUp component from the parent GameObject
        catUnit = shieldUp.GetComponent<CatUnit>(); // Get the CatUnit component from the ShieldUp GameObject
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy") && shieldUp.shieldHealth >= 0)
            {
                if(animationPlaying == false)
                {
                    // Play the shield hit animation here
                    // You can use an Animator component to trigger the animation
                    Animator animator = GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetTrigger("Hit"); // Assuming you have a trigger parameter named "Hit" in your Animator
                        animationPlaying = true; // Set the flag to indicate that the animation is playing
                    }
                }
                // Handle collision with enemy
                // For example, reduce shield health and apply damage to the enemy
                Debug.Log("Shield collided with an enemy!");
                // You can implement the logic to reduce shield health and apply damage here
                EnemyUnit enemy = collision.gameObject.GetComponent<EnemyUnit>();
                
                //play animation
                enemy.TakeDamage(catUnit, 1, 10f);

                shieldUp.shieldHealth -= 1; // Reduce the shield value by 1 when it collides with an enemy

                if (shieldUp.shieldHealth <= 0)
                {
                    // Destroy the shield GameObject when the shield value reaches 0
                    shieldUp.DestroyShield();
                }
            }
        }
    }

    void AnimationFinish() //there will be a flag in spine2D to call this.
    {
        animationPlaying = false; // Reset the flag when the animation finishes
    }
}
