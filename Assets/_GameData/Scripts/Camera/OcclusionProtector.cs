using UnityEngine;

public class OcclusionProtector : MonoBehaviour {    
    //for constants
    private const float MIN_DISTANCE_TO_PLAYER = 1f;
    private const float MAX_DISTANCE_TO_PLAYER = 5f;
    private const float MIN_NEAR_CLIP_PLANE_EXTENT_MULTIPLIER = 1f;
    private const float MAX_NEAR_CLIP_PLANE_EXTENT_MULTIPLIER = 2f;
    private const float MIN_OCCLUSION_MOVE_TIME = 0f;
    private const float MAX_OCCLUSION_MOVE_TIME = 1f;

    private struct ClipPlaneCornerPoints {
        public Vector3 UpperLeft { get; set; }
        public Vector3 UpperRight { get; set; }
        public Vector3 LowerLeft { get; set; }
        public Vector3 LowerRight { get; set; }
    }

    // for public fields
    [Range(MIN_DISTANCE_TO_PLAYER, MAX_DISTANCE_TO_PLAYER)]
    [Tooltip("The original distance to target (in meters)")]
    public float distanceToTarget = 2.5f; // In meters

    [Range(MIN_NEAR_CLIP_PLANE_EXTENT_MULTIPLIER, MAX_NEAR_CLIP_PLANE_EXTENT_MULTIPLIER)]
    [Tooltip("Higher values ensure better occlusion protection, but decrease the distance between the camera and the target")]
    public float nearClipPlaneExtentMultiplier = 1.2f;

    [Range(MIN_OCCLUSION_MOVE_TIME, MAX_OCCLUSION_MOVE_TIME)]
    [Tooltip("The time needed for the camera to reach secure position when an occlusion occurs (in seconds)")]
    public float occlusionMoveTime = 0.025f; // The lesser, the better

    [Tooltip("What objects should the camera ignore when checked for clips and occlusions")]
    public LayerMask ignoreLayerMask = 0; // What objects should the camera ignore when checked for clips and occlusions

#if UNITY_EDITOR
    public bool visualizeInScene = true;
#endif

    // Private fields
    private new Camera camera;
    private Transform pivot; // The point at which the camera pivots around
    private Vector3 cameraVelocity;
    private float nearClipPlaneHalfHeight;
    private float nearClipPlaneHalfWidth;
    private float sphereCastRadius;

    void Awake() {
        camera = this.GetComponent<Camera>();
        pivot = transform.parent;

        float halfFOV = (camera.fieldOfView / 2.0f) * Mathf.Deg2Rad; // vertical FOV in radians
        
        nearClipPlaneHalfHeight = Mathf.Tan(halfFOV) * camera.nearClipPlane * nearClipPlaneExtentMultiplier;
        nearClipPlaneHalfWidth = nearClipPlaneHalfHeight * camera.aspect;
        sphereCastRadius = new Vector2(nearClipPlaneHalfWidth, nearClipPlaneHalfHeight).magnitude; // Pythagoras
    }

    void Update() {
        UpdateCameraPosition();

#if UNITY_EDITOR
        DrawDebugVisualization();
#endif
    }

#if UNITY_EDITOR
    private void DrawDebugVisualization() {
 
        if (visualizeInScene)
        {
            ClipPlaneCornerPoints nearClipPlaneCornerPoints = GetNearClipPlaneCornerPoints(transform.position);

            Debug.DrawLine(pivot.position, transform.position - transform.forward * camera.nearClipPlane, Color.red);
            Debug.DrawLine(pivot.position, nearClipPlaneCornerPoints.UpperLeft, Color.green);
            Debug.DrawLine(pivot.position, nearClipPlaneCornerPoints.UpperRight, Color.green);
            Debug.DrawLine(pivot.position, nearClipPlaneCornerPoints.LowerLeft, Color.green);
            Debug.DrawLine(pivot.position, nearClipPlaneCornerPoints.LowerRight, Color.green);
            Debug.DrawLine(nearClipPlaneCornerPoints.UpperLeft, nearClipPlaneCornerPoints.UpperRight, Color.green);
            Debug.DrawLine(nearClipPlaneCornerPoints.UpperRight, nearClipPlaneCornerPoints.LowerRight, Color.green);
            Debug.DrawLine(nearClipPlaneCornerPoints.LowerRight, nearClipPlaneCornerPoints.LowerLeft, Color.green);
            Debug.DrawLine(nearClipPlaneCornerPoints.LowerLeft, nearClipPlaneCornerPoints.UpperLeft, Color.green);
        }
    }
#endif

    /// <summary>
    /// Checks if the camera is Occluded.
    /// </summary>
    /// <param name="cameraPosition"> The position of the camera</param>
    /// <param name="outDistanceToTarget"> if the camera is occluded, the new distance to target is saved in this variable</param>
    /// <returns></returns>
    private bool IsCameraOccluded(Vector3 cameraPosition, ref float outDistanceToTarget)
    {
        // Cast a sphere along a ray to see if the camera is occluded
        Ray ray = new Ray(pivot.transform.position, -transform.forward);
        float rayLength = distanceToTarget - camera.nearClipPlane;
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereCastRadius, out hit, rayLength, ~ignoreLayerMask))
        {
            outDistanceToTarget = hit.distance + sphereCastRadius;
            return true;
        }
        else
        {
            outDistanceToTarget = -1f;
            return false;
        }
    }

    private void UpdateCameraPosition() {

        Vector3 newCameraLocalPosition = transform.localPosition;
        newCameraLocalPosition.z = -distanceToTarget;
        Vector3 newCameraPosition = pivot.TransformPoint(newCameraLocalPosition);
        float newDistanceToTarget = distanceToTarget;
        
        if (IsCameraOccluded(newCameraPosition, ref newDistanceToTarget))
        {
            newCameraLocalPosition.z = -newDistanceToTarget;
            newCameraPosition = pivot.TransformPoint(newCameraLocalPosition);
        }
        
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition, newCameraLocalPosition, ref cameraVelocity, occlusionMoveTime);
    }

    private ClipPlaneCornerPoints GetNearClipPlaneCornerPoints(Vector3 cameraPosition)
    {
        ClipPlaneCornerPoints nearClipPlanePoints = new ClipPlaneCornerPoints();

        nearClipPlanePoints.UpperLeft = cameraPosition - transform.right * nearClipPlaneHalfWidth;
        nearClipPlanePoints.UpperLeft += transform.up * nearClipPlaneHalfHeight;
        nearClipPlanePoints.UpperLeft += transform.forward * camera.nearClipPlane;

        nearClipPlanePoints.UpperRight = cameraPosition + transform.right * nearClipPlaneHalfWidth;
        nearClipPlanePoints.UpperRight += transform.up * nearClipPlaneHalfHeight;
        nearClipPlanePoints.UpperRight += transform.forward * camera.nearClipPlane;

        nearClipPlanePoints.LowerLeft = cameraPosition - transform.right * nearClipPlaneHalfWidth;
        nearClipPlanePoints.LowerLeft -= transform.up * nearClipPlaneHalfHeight;
        nearClipPlanePoints.LowerLeft += transform.forward * camera.nearClipPlane;

        nearClipPlanePoints.LowerRight = cameraPosition + transform.right * nearClipPlaneHalfWidth;
        nearClipPlanePoints.LowerRight -= transform.up * nearClipPlaneHalfHeight;
        nearClipPlanePoints.LowerRight += transform.forward * camera.nearClipPlane;

        return nearClipPlanePoints;
    }
    
    private void OnDrawGizmos() {
       if (Application.isPlaying)
       {
           Gizmos.color = Color.yellow;
           Gizmos.DrawSphere(pivot.transform.position - (transform.forward * (distanceToTarget - camera.nearClipPlane)), sphereCastRadius);
       }
    }
}
