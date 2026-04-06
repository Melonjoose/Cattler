using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Collider2D))] // or Collider for 3D
public class MovementDrag : MonoBehaviour
{
    public CatMovement catMovement;
    public Camera mainCam;
    private LineRenderer line;
    private Collider2D col;

    private bool dragging = false;
    private float camToObjDistance;
    private Vector3 startPos;
    private Vector3 lastMouseWorld;

    public float arcHeight = 1.5f;
    public int resolution = 20;

    public float snapRange = 2f;

    public Vector3 startPositionOffset;
    public GameObject arrowHeadPrefab;
    private GameObject arrowHeadInstance;

    void Awake()
    {
        catMovement = GetComponent<CatMovement>();
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution + 1;
        line.useWorldSpace = true;
        line.enabled = false;
        col = this.GetComponent<Collider2D>();

        mainCam = Camera.main;
        if (mainCam == null) Debug.LogError("No main camera found (tag MainCamera).");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Only raycast against the "Cats" layer
            int catLayerMask = LayerMask.GetMask("Cats");
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, catLayerMask);

            if (hit.collider != null && hit.collider == col)
            {
                dragging = true;
                camToObjDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
                Debug.Log("Cat clicked!");
            }
        }

        if (Input.GetMouseButton(0) && dragging)
        {
            startPos = transform.position + startPositionOffset;
            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = camToObjDistance;
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = 0;

            lastMouseWorld = mouseWorld;
            DrawArc(startPos, mouseWorld);
            SnapArrowToPosition();
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            line.enabled = false;
            HandleRelease();
        }


    }

    void HandleRelease()
    {
        CatUnit otherCat;
        int nearestIndex = FindNearestPositionIndex(lastMouseWorld, out otherCat);

        Debug.Log($"other cat is " + otherCat);
        if (nearestIndex != -1)
        {
            int originalIndex = catMovement.catIndex;

            if (otherCat != null) // another cat owns that slot
            {
                Debug.Log(otherCat + " is found");

                // Swap logical ownership
                catMovement.catIndex = nearestIndex;
                otherCat.catMovement.catIndex = originalIndex;

                // Move them to their designated slots (regardless of current physical position)
                catMovement.MoveToDesignatedLocation(nearestIndex);
                otherCat.catMovement.MoveToDesignatedLocation(originalIndex);
            }
            else
            {
                Debug.Log("no cat is found");

                // Just move current cat to new slot
                catMovement.catIndex = nearestIndex;
                catMovement.MoveToDesignatedLocation(nearestIndex);
            }
        }

        arrowHeadInstance.SetActive(false);
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

            if (Vector3.Distance(cat.transform.position, catMovement.worldPositions[nearestIndex].position) < 0.5f)
            {
                otherCat = cat;
                break;
            }
        }

        return nearestIndex; // +1 since your movement uses 0-based indexing
    }


    void SnapArrowToPosition()
    {
        float nearestDist = Mathf.Infinity;
        int nearestIndex = -1;

        for (int i = 0; i < catMovement.worldPositions.Count; i++)
        {
            float dist = Vector3.Distance(lastMouseWorld, catMovement.worldPositions[i].position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestIndex = i;
            }
        }

        // If within snap range, redraw arc to snapped position
        if (nearestDist <= snapRange && nearestIndex != -1)
        {
            Vector3 snappedTip = catMovement.worldPositions[nearestIndex].position;
            DrawArc(startPos, snappedTip);
        }
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

        // Place arrowhead at the end
        if (arrowHeadInstance == null)
        {
            arrowHeadInstance = Instantiate(arrowHeadPrefab);
        }

        arrowHeadInstance.SetActive(true);
        arrowHeadInstance.transform.position = end;
        // Rotate to face direction
        Vector3 dir = (end - line.GetPosition(resolution - 1)).normalized;
        arrowHeadInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
        new GradientColorKey(new Color(0.925f, 0.573f, 0.286f), 0.0f), // start color
        new GradientColorKey(new Color(0.925f, 0.573f, 0.286f), 1.0f) // EC9249 orange at end
            },
            new GradientAlphaKey[] {
        new GradientAlphaKey(1.0f, 0.0f), // fully opaque at start
        new GradientAlphaKey(1.0f, 1.0f)  // fully opaque at end
            }
        );
        line.colorGradient = gradient;


        line.sortingLayerName = "Default"; // or a custom layer like "Background"
        line.sortingOrder = -1;             // lower number = behind

    }
}
