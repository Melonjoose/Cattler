using UnityEngine;

public class VFXreturner : MonoBehaviour
{
    public int prefabIndex; // set this in inspector or dynamically

    // Called by Animation Event
    public void OnAnimationComplete()
    {
        StatFXManager.instance.ReturnVFXToPool(gameObject, prefabIndex);
    }
}

