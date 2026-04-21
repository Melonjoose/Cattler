using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
[System.Serializable]
public class DebuffInstance
{
    public Unit unit; // The unit this debuff is applied to
    public Debuff debuff;

    public Sprite sprite; // For UI display, if needed
    public GameObject debuffIcon;

    public float remainingTime;

    public DebuffInstance(Debuff debuff, Unit unit, float duration)
    {
        this.debuff = debuff;
        this.unit = unit;
        this.sprite = debuff.debuffIcon;
        this.remainingTime = duration;
    }


    public bool Tick(float deltaTime)
    {
        remainingTime -= deltaTime;
        return remainingTime <= 0;
    }

    public void ShowUI()
    {
        // Implement UI display logic here, e.g., show debuff icon above unit
        // show UI above enemy head. when the debuff is at the last 1 second, the icon will start flashing. when the debuff is removed, the icon will disappear.
        if(unit != null && sprite != null)
        {
            // Example: Instantiate a UI element above the unit and set the sprite
            // This is a placeholder and would need to be implemented based on your specific UI setup
            Debug.Log($"Showing debuff {debuff.debuffName} on {unit.name}");
            debuffIcon = GameObject.Instantiate(DebuffManager.instance.iconPrefab, unit.transform.position + Vector3.up * 2, Quaternion.identity);
            //set debufficon as a child of this unit's.debuffcanvas
            debuffIcon.transform.SetParent(unit.GetComponent<Unit>().debuffCanvas.transform, false);
            debuffIcon.name = $"{debuff.debuffName} Icon"; // Name the icon for easier debugging
            Image image = debuffIcon.GetComponent<Image>();

            if (image != null)
            {
                image.sprite = sprite;
            }
        }
    }

    public void BlinkingUI()
    {
        if (debuffIcon == null) return;

        Image image = debuffIcon.GetComponent<Image>();
        if (image == null) return;

        // Blink speed (how many times per second)
        float blinkSpeed = 6f;

        // Ping-pong between 0 and 1 based on time
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void RemoveUI()
    {
        if (debuffIcon != null)
        {
            GameObject.Destroy(debuffIcon);
            debuffIcon = null;
        }
    }


    public void ApplyDebuffEffect() //apply this debuffeffect
    {
        ShowUI();
        Stun();
        Slow();
    }

    public void RemoveDebuffEffect()
    {
        UnStun();
        RemoveSlow();
    }


    //------debuff effects on unit -------//

    //stun effect.
    void Stun()
    {
        if (debuff.debuffType == Debuff.DebuffType.Stun) //checking this debuff type
        {
            unit.isStunned = true;
            unit.canAttack = false;
            unit.canWalk = false;
            if(unit.isDead == false)
            {
                unit.skeletonAnimation.AnimationState.SetAnimation(0, "Hit", false);
            }
        }     
    }

    void UnStun()
    {
        if (debuff.debuffType == Debuff.DebuffType.Stun)
        {
            unit.isStunned = false;
            unit.canAttack = true;
            unit.canWalk = true;
            if (unit.isDead == false && unit.canWalk)
            {
                unit.skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
            }
            else
            {
                unit.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
        }

    }

    private float originalMoveSpeed;
    void Slow()
    {
        if (debuff.debuffType == Debuff.DebuffType.Slow && !unit.isSlowed)
        {
            EnemyUnit enemyUnit = unit as EnemyUnit;
            originalMoveSpeed = enemyUnit.EnemyMovement.moveSpeed;
            enemyUnit.EnemyMovement.moveSpeed *= 0.5f; // reduce by 50%
            unit.isSlowed = true;
        }
    }

    void RemoveSlow()
    {
        if (debuff.debuffType == Debuff.DebuffType.Slow)
        {
            if (unit.isSlowed)
            {
                EnemyUnit enemyUnit = unit as EnemyUnit;
                enemyUnit.EnemyMovement.moveSpeed = originalMoveSpeed;
                unit.isSlowed = false;
            }
        }
        RemoveUI();
    }
}