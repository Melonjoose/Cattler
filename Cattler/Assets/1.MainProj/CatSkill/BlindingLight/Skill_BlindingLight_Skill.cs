using UnityEngine;

public class BlindingLight : CatSkill
{
    public LightAura lightAura; // Reference to the LightAura component that handles the visual effect of the blinding light
    //cooldown is for the skill intervals
    //value is the stun duration
    //speed is the expansion speed of the blinding light effect
    //itemobject is the blinding light effect prefab called lightaura
    void Start()
    {
        lightAura = GetComponentInChildren<LightAura>(); // Get the LightAura component from the child object
        lightAura.gameObject.SetActive(false); // Ensure the light aura effect is initially inactive
    }


    public override void UseSkill()
    {
        // Implement the logic for Blinding Light skill here
        lightAura.gameObject.SetActive(true); // Activate the light aura effect
        AudioManager.instance.PlaySFX("BlindingLight"); // Play the blinding light sound effect (make sure to have this sound in your AudioManager)
    }
}
