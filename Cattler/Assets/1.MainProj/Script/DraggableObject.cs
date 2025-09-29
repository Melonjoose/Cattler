using UnityEngine;

public class DragWorldObject : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody2D rb;
    private bool isDragging = false;
    public float followSpeed = 10f; // higher = faster catch-up, lower = more lag

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            rb.isKinematic = true;
            // Mouse position > world position
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                mainCam.WorldToScreenPoint(transform.position).z)
            );

            mouseWorldPos.z = transform.position.z; // keep original depth

            // Smooth follow instead of snapping
            transform.position = Vector3.Lerp(transform.position, mouseWorldPos, followSpeed * Time.deltaTime);
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
