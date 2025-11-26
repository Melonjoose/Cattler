using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public Collider2D col;
    public void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (TravelManager.instance.disableTravel == true) return;
        if (TravelManager.instance.isTraveling != true) return;
        transform.position += Vector3.left * (TravelManager.instance.travelSpeed*TravelManager.instance.distanceMultiplier) * Time.deltaTime;
    }

}
