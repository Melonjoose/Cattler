using UnityEngine;

public class ActiveAbility : MonoBehaviour
{
    public GameObject AbilityPrefab;

    public int damage;  //double has healing 
    public float cooldown;
    public float range;
    public float movespeed = 3f;
    public float lifetime = 0.7f;
    public float timer = 0f;
    public Collider2D col;
    public Rigidbody2D rb;

    public CatUnit catUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachSkillToCat(CatUnit cat)
    {
        catUnit = cat;
    }
}
