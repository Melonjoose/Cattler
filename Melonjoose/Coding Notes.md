| **Type**                 | **Purpose** | **Lives in**       | **Used for**            |
| ------------------------ | ----------- | ------------------ | ----------------------- |
| `MonoBehaviour`          | Behavior    | Scene (GameObject) | Logic, updates, physics |
| `ScriptableObject`       | Shared data | Asset file         | Templates, static data  |
| Plain `class` / `struct` | Pure data   | In memory          | Runtime instances       |
|                          |             |                    |                         |

    public virtual void Die()  
    {   UnHookCat(); //plays this IF no override

    public override void Die()  
    {
        UnHookCat();


        base.Die();  // Call base Die method to handle death
        //SpecialEnemySpawner.instance.RemoveSpawnedEnemies(this.gameObject);
        //Currency.instance.AddInk(10); // Add ink to currency
        //dropLoot.GiveLoot();
        //Destroy(gameObject);
    }
    
//USING OVERRIDE, Can use base.Die() to just call the inherited function without typing everything out again.

Class -> blueprint / template
Objects -> Instantiated from class. (individual entity itself)


        if (cat == this.GetComponent<CatUnit>()) continue; // skip self
//LEARN THE EXISTENCE OF continue


private int FindNearestPositionIndex(Vector3 mouseWorld, out CatUnit otherCat)

//KNOW about out

**Confirm subscription happens before `Consume()` is invoked**. Add logs:

- In each subclass `OnEnable()` / `Start()` print `Subscribed`.
    
- In `ConsumableItem.Consume()` print before `onConsumed?.Invoke()` (you already have VFX log).
    
- If “Subscribed” doesn’t appear before Consume, the listener isn’t attached.
//CAN ADD LISTENER CHECKER