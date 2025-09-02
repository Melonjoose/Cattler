using UnityEngine;

public class DebugBoxCollider : MonoBehaviour
{
    public bool toggleBox = true;
    public Vector3 boxSize = new Vector3(1, 1, 1);
    public Color boxColor = Color.green;
    public Transform boxCenter;

    private void OnDrawGizmos()
    {
        if (!toggleBox) return;

        Gizmos.color = boxColor;

        if (boxCenter != null)
        {
            // Draw box centered at the boxCenter transform
            Gizmos.DrawWireCube(boxCenter.position, boxSize);
        }
        else
        {
            // Default: center at this GameObject
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}
