using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public CapsuleCollider2D col;
    public void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (TravelManager.instance.isTraveling != true) return;
        transform.position += Vector3.left * (TravelManager.instance.travelSpeed*TravelManager.instance.distanceMultiplier) * Time.deltaTime;
    }

}
