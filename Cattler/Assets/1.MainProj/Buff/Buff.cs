using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Status/Buff")]
public class Buff : ScriptableObject
{
    public string buffName;
    public Sprite sprite;
    public float duration;
    public BuffType buffType;
    public ValueIncreaseType valueIncreaseType;
    public enum ValueIncreaseType {Flat , Percentage};
    public float value; // can be use as a flat or percentage incease.

    public enum BuffType
    {
        AttackSpeed,
        AttackPower,
        Range,
        Defense,
        CooldownReduction,
        Heal,
        // Add more buff types as needed
    }
}