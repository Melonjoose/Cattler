using UnityEngine;
[System.Serializable]
public class DebuffInstance
{
    public Debuff debuff;
    public Sprite sprite; // For UI display, if needed
    public float remainingTime;

    public DebuffInstance(Debuff debuff)
    {
        this.debuff = debuff;
        this.sprite = debuff.debuffIcon;
        this.remainingTime = debuff.duration;
    }

    public bool Tick(float deltaTime)
    {
        remainingTime -= deltaTime;
        return remainingTime <= 0;
    }
}