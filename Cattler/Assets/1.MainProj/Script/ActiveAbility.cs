using UnityEngine;

public class ActiveAbility : MonoBehaviour
{
    public GameObject AbilityPrefab;

    public int damage;  //double has healing // might be useless because damage is calculated based on cat's attack power. but can be used for flat damage skills or healing skills.
    public Vector3 spawnLocationOffset; //offset from the cat's position where the skill will spawn //default is usually (1 , 0 ,0).
    public float cooldown;
    public float range;
    public float movespeed = 3f;
    public float lifetime = 0.7f;
    public float timer = 0f;
    public float damageMultiplier = 1.2f; //default multiplier for damage calculation, can be adjusted for different skills
    public Collider2D col;
    public Rigidbody2D rb;

    public CatUnit catUnit;

    public AudioClip skillSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catUnit = GetComponent<CatUnit>();
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
