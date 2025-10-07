| **Type**                 | **Purpose** | **Lives in**       | **Used for**            |
| ------------------------ | ----------- | ------------------ | ----------------------- |
| `MonoBehaviour`          | Behavior    | Scene (GameObject) | Logic, updates, physics |
| `ScriptableObject`       | Shared data | Asset file         | Templates, static data  |
| Plain `class` / `struct` | Pure data   | In memory          | Runtime instances       |
|                          |             |                    |                         |


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