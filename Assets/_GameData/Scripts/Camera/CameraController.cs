using UnityEngine;

public class CameraController : MonoBehaviour {
    //for the constant
    const float MIN_CATCH_SPEED_DAMP = 0f;
    const float MAX_CATCH_SPEED_DAMP = 1f;
    const float MIN_ROTATION_SMOOTHING = 0f;
    const float MAX_ROTATION_SMOOTHING = 30f;

    //for the public variables
    public Transform target = null; // The target to follow
    
    [Range(MIN_CATCH_SPEED_DAMP, MAX_CATCH_SPEED_DAMP)]
    public float catchSpeedDamp = MIN_CATCH_SPEED_DAMP;

    [Range(MIN_ROTATION_SMOOTHING, MAX_ROTATION_SMOOTHING)]
    [Tooltip("How fast the camera rotates around the pivot")]
    public float rotationSmoothing = 15.0f;
    
    //private variables
    Transform rig; // The root transform of the camera rig
    Transform pivot; // The point at which the camera pivots around
    Quaternion pivotTargetLocalRotation; // Controls the X Rotation (Tilt Rotation)
    Quaternion rigTargetLocalRotation; // Controlls the Y Rotation (Look Rotation)
    Vector3 cameraVelocity; // The velocity at which the camera moves

    public RotationPanel myRotationPanel; //this panel will help user to rotate at any angle
    public Joystick myJoyStick;

    bool isCameraAutoRotate = true;
    public static bool stopCamera = false;
    
    void Awake() {
        pivot = transform.parent;
        rig = pivot.parent;
    }

    //void Start(){
    //    myRotationPanel = GameObject.FindObjectOfType<RotationPanel>();
    //    myJoyStick = GameObject.FindObjectOfType<Joystick>();
    //}
    
    Quaternion lastCharacterRotation;
    Quaternion controlRotation;
    void Update() {

       
    }

    Vector3 playerPrevPos = Vector3.zero;
    void LateUpdate() {

        if (!stopCamera)
        {
            // for the rotation of camera angle according to the user rotatation panel input
            if (myRotationPanel.Pressed)
            {
                controlRotation = InputController.GetMouseRotationInput();
                UpdateRotation(controlRotation);
            }
            else
            {
                controlRotation = InputController.GetControllerRotationInput();
                UpdateRotation(controlRotation);
            }
        }

        FollowTarget();
    }
    
    void FollowTarget() {
        rig.position = Vector3.SmoothDamp(rig.position, target.transform.position, ref cameraVelocity, catchSpeedDamp);
    }

    void UpdateRotation(Quaternion controlRotation) {
        // Y Rotation (Look Rotation)
        rigTargetLocalRotation = Quaternion.Euler(0f, controlRotation.eulerAngles.y, 0f);

        // X Rotation (Tilt Rotation)
        pivotTargetLocalRotation = Quaternion.Euler(controlRotation.eulerAngles.x, 0f, 0f);

        if (rotationSmoothing > 0.0f) {
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, 
                                        pivotTargetLocalRotation, rotationSmoothing * Time.deltaTime);

            rig.localRotation = Quaternion.Slerp(rig.localRotation, 
                                      rigTargetLocalRotation, rotationSmoothing * Time.deltaTime);
        }
        else {
            pivot.localRotation = pivotTargetLocalRotation;
            rig.localRotation = rigTargetLocalRotation;
        }
    }

    public void ActiveCameraAutoRotation(){
        isCameraAutoRotate = !isCameraAutoRotate;
    }
}