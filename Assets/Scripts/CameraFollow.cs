using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Player
    public Vector3 offset = new Vector3(0, 0, -10f);
    [Range(0, 1)] public float smoothSpeed = 0.125f;

    [Header("Optional Bounds")]
    public BoxCollider2D levelBounds; 

    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            cam = Camera.main;

        // If a boundary collider is assigned, use its bounds
        if (levelBounds != null)
        {
            Bounds bounds = levelBounds.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;
        }

        // Calculate camera extents
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Desired camera position
        Vector3 desiredPosition = target.position + offset;

        // Clamp position only if bounds are set
        if (levelBounds != null)
        {
            float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
            desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        }

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (levelBounds != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(levelBounds.bounds.center, levelBounds.bounds.size);
        }
    }
#endif
}




