using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Collider2D))] // or Collider for 3D
public class MovementDrag : MonoBehaviour
{
    public CatMovement catMovement;
    public Camera mainCam;
    private LineRenderer line;


    private bool dragging = false;
    private float camToObjDistance;
    private Vector3 startPos;
    private Vector3 lastMouseWorld;

    public float arcHeight = 1.5f;
    public int resolution = 20;

    public float snapRange = 2f;

    void Awake()
    {
        catMovement = GetComponent<CatMovement>();
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution + 1;
        line.useWorldSpace = true;
        line.enabled = false;

        mainCam = Camera.main;
        if (mainCam == null) Debug.LogError("No main camera found (tag MainCamera).");
    }

    void OnMouseDown()
    {
        // Called when mouse button pressed over this object's collider
        dragging = true;
        camToObjDistance = Vector3.Distance(mainCam.transform.position, transform.position);
    }

    private void OnMouseDrag()
    {
        if (!dragging) return;

        // Draw line from object to arrow tip at mouse position.
        // if there are any nearby positions, snap arrow tip to that position.
        
        startPos = transform.position;
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = camToObjDistance;
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0; // flatten to 2D plane
        Vector3 arrowTip = mouseWorld;

        lastMouseWorld = mouseWorld;
        DrawArc(startPos, arrowTip);

        SnapArrowToPosition();
    }


    private void OnMouseUp()
    {
        dragging = false;
        line.enabled = false;

        CatUnit otherCat;
        int nearestIndex = FindNearestPositionIndex(lastMouseWorld, out otherCat);

        if (nearestIndex != -1)
        {
            if(otherCat != null)
            {
                int originalIndex = catMovement.catIndex;
                catMovement.catIndex = nearestIndex;
                otherCat.catMovement.catIndex = originalIndex;
            }
            else
            {
                catMovement.MoveToDesignatedLocation(nearestIndex);
                Debug.Log($"Moving to newPosition {nearestIndex}");
            }
        }
    }


    private int FindNearestPositionIndex(Vector3 mouseWorld, out CatUnit otherCat)
    {
        float nearestDist = Mathf.Infinity;
        int nearestIndex = -1;
        otherCat = null;

        for (int i = 0; i < catMovement.worldPositions.Count; i++)
        {
            float dist = Vector3.Distance(mouseWorld, catMovement.worldPositions[i].position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestIndex = i;
            }
        }

        if (nearestDist > snapRange) // adjust range to taste
            return -1;

        foreach (CatUnit cat in FindObjectsByType<CatUnit>(FindObjectsSortMode.None))
        {
            if (cat == this.GetComponent<CatUnit>()) continue; // skip self

            if (Vector3.Distance(cat.transform.position, catMovement.worldPositions[nearestIndex].position) < 0.1f)
            {
                otherCat = cat;
                break;
            }
        }

        return nearestIndex + 1; // +1 since your movement uses 1-based indexing
    }


    void SnapArrowToPosition()
    {
        //base on distance to nearest position, snap arrow to that position.

    }
    private void DrawArc(Vector3 start, Vector3 end)
    {
        line.enabled = true;
        line.startColor = Color.red;
        line.endColor = Color.yellow;

        Vector3 mid = (start + end) / 2 + Vector3.up * arcHeight;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 p1 = Vector3.Lerp(start, mid, t);
            Vector3 p2 = Vector3.Lerp(mid, end, t);
            Vector3 curvePoint = Vector3.Lerp(p1, p2, t);
            line.SetPosition(i, curvePoint);
        }
    }
}
