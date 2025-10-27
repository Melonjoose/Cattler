using UnityEngine;
using System;
public class ContainerDetector : MonoBehaviour
{
    public CatUnit occupyingCat;
    public int containerIndex;
    public bool IsOccupied => occupyingCat != null;
}
