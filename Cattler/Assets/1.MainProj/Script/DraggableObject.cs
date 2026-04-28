using UnityEngine;

public class DragWorldObject : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody2D rb;
    private bool isDragging = false;
    public float followSpeed = 20f; // higher = faster catch-up, lower = more lag

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        if (HoverHighlightManager.instance != null)
        {
            HoverHighlightManager.instance.isDragging = false;
        }
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
            rb.bodyType = RigidbodyType2D.Kinematic;
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
             rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
