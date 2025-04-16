using UnityEngine;

[CreateAssetMenu(fileName = "NewCat", menuName = "Units/Cat")]
public class CatData : ScriptableObject
{
    public string catName;
    public Sprite icon;
    public int health;
    public int attackPower;
    public float attackSpeed;
    //public AbilityData[] abilities; // Optional
}