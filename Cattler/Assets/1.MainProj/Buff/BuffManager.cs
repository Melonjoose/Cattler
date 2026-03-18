using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    private List<Buff> activeBuffs = new List<Buff>();

    void Awake()
    {
        instance = this;
    }

    public void ApplyBuff(Buff buff, CatUnit cat)
    {
        activeBuffs.Add(buff);
        buff.Apply(cat);
        StartCoroutine(RemoveBuffAfterDuration(buff , cat));
    }

    private IEnumerator RemoveBuffAfterDuration(Buff buff, CatUnit cat)
    {
        yield return new WaitForSeconds(buff.duration);
        buff.Remove(cat);
        activeBuffs.Remove(buff);
        Debug.Log($"{cat.name}'s {buff.buffName} expired.");
    }
}
