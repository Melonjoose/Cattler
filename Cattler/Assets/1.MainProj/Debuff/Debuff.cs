using UnityEngine;

[CreateAssetMenu(fileName = "Debuff", menuName = "Status/Debuff")]
public class Debuff : ScriptableObject
{
    public string debuffName;
    public string description;
    public Sprite debuffIcon;
    public float duration;

    // Optional: define what the debuff does
    public enum DebuffType { Stun, Slow, Poison }
    public DebuffType debuffType;

    public float effectStrength; // e.g., slow % or poison damage //if 100% then it would be a full stun, if 50% then it would be a 50% slow, etc.
}