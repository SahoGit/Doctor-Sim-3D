using UnityEngine;

public class Character : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    private new Camera camera = null;

    [SerializeField]
    private MovementSettings movementSettings = null;

    [SerializeField]
    private GravitySettings gravitySettings = null;

    [SerializeField]
    [HideInInspector]
    private RotationSettings rotationSettings = null;

    //public fields
    public Transform focusPoint;
    public LayerMask detectableMasks;

    // Private fields
    private Vector3 moveVector;
    private Quaternion controlRotation;
    private CharacterController controller;
    private bool isWalking;
    private bool isJogging;
    private bool isSprinting;
    private float maxHorizontalSpeed; // In meters/second
    private float targetHorizontalSpeed; // In meters/second
    private float currentHorizontalSpeed; // In meters/second
    private float currentVerticalSpeed; // In meters/second

    public static Joystick myJoystick;
    
    //---------------
    [Header("What is What:")]
    public string doorTag;

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        // Gizmos.DrawLine(transform.position, transform.forward * 20f);
        Gizmos.DrawRay(new Ray(focusPoint.position, transform.forward * 20f));
    }

    #region Unity Methods
    protected virtual void Awake() {
        controller = this.GetComponent<CharacterController>();

        CurrentState = CharacterStateBase.GROUNDED_STATE;
        IsJogging = true;
    }

    void Start(){
        myJoystick = GameObject.FindObjectOfType<Joystick>();
    }

    void OnEnable(){
        CharacterAnimator.isCharacterControlsEnabled = true;
    }
    void OnDisable(){
        CharacterAnimator.isCharacterControlsEnabled = false;
    }

    protected virtual void Update() {
        CurrentState.Update(this);

        //check for doors
        CheckDoors();

        UpdateHorizontalSpeed();
        ApplyMotion();
    }

    #endregion Unity Methods

    public ICharacterState CurrentState { get; set; }

    public Vector3 MoveVector
    {
        get
        {
            return moveVector;
        }
        set
        {
            float moveSpeed = value.magnitude * maxHorizontalSpeed;
            if (moveSpeed < Mathf.Epsilon)
            {
                targetHorizontalSpeed = 0f;
                return;
            }
            else if (moveSpeed > 0.01f && moveSpeed <= MovementSettings.WalkSpeed)
            {
                targetHorizontalSpeed = MovementSettings.WalkSpeed;
            }
            else if (moveSpeed > MovementSettings.WalkSpeed && moveSpeed <= MovementSettings.JogSpeed)
            {
                targetHorizontalSpeed = MovementSettings.JogSpeed;
            }
            else if (moveSpeed > MovementSettings.JogSpeed)
            {
                targetHorizontalSpeed = MovementSettings.SprintSpeed;
            }

            moveVector = value;
            if (moveSpeed > 0.01f)
            {
                moveVector.Normalize();
            }
        }
    }

    public Camera Camera
    {
        get
        {
            return camera;
        }
    }

    public CharacterController Controller
    {
        get
        {
            return controller;
        }
    }

    public MovementSettings MovementSettings
    {
        get
        {
            return movementSettings;
        }
        set
        {
            movementSettings = value;
        }
    }

    public GravitySettings GravitySettings
    {
        get
        {
            return gravitySettings;
        }
        set
        {
            gravitySettings = value;
        }
    }

    public RotationSettings RotationSettings
    {
        get
        {
            return rotationSettings;
        }
        set
        {
            rotationSettings = value;
        }
    }

    public Quaternion ControlRotation
    {
        get
        {
            return controlRotation;
        }
        set
        {
            controlRotation = value;
            AlignRotationWithControlRotationY();
        }
    }

    public bool IsWalking
    {
        get
        {
            return isWalking;
        }
        set
        {
            isWalking = value;
            if (isWalking)
            {
                maxHorizontalSpeed = MovementSettings.WalkSpeed;
                IsJogging = false;
                IsSprinting = false;
            }
        }
    }

    public bool IsJogging
    {
        get
        {
            return isJogging;
        }
        set
        {
            isJogging = value;
            if (isJogging)
            {
                maxHorizontalSpeed = MovementSettings.JogSpeed;
                IsWalking = false;
                IsSprinting = false;
            }
        }
    }
    public bool IsSprinting
    {
        get
        {
            return isSprinting;
        }
        set
        {
            isSprinting = value;
            if (isSprinting)
            {
                maxHorizontalSpeed = MovementSettings.SprintSpeed;
                IsWalking = false;
                IsJogging = false;
            }
            else
            {
                if (!(IsWalking || IsJogging))
                {
                    IsJogging = true;
                }
            }
        }
    }

    public bool IsGrounded
    {
        get
        {
            return controller.isGrounded;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return controller.velocity;
        }
    }

    public Vector3 HorizontalVelocity
    {
        get
        {
            return new Vector3(Velocity.x, 0f, Velocity.z);
        }
    }

    public Vector3 VerticalVelocity
    {
        get
        {
            return new Vector3(0f, Velocity.y, 0f);
        }
    }

    public float HorizontalSpeed
    {
        get
        {
            return new Vector3(Velocity.x, 0f, Velocity.z).magnitude;
        }
    }

    public float VerticalSpeed
    {
        get
        {
            return Velocity.y;
        }
    }

    public void Jump()
    {
        currentVerticalSpeed = MovementSettings.JumpForce;
    }

    public void ToggleWalk()
    {
        IsWalking = !IsWalking;

        if (!(IsWalking || IsJogging))
        {
            IsJogging = true;
        }
    }

    public void ApplyGravity(bool isGrounded = false)
    {
        if (!isGrounded)
        {
            currentVerticalSpeed =
                MathfExtensions.ApplyGravity(VerticalSpeed, GravitySettings.GravityStrength, GravitySettings.MaxFallSpeed);
        }
        else
        {
            currentVerticalSpeed = -GravitySettings.GroundedGravityForce;
        }
    }

    public void ResetVerticalSpeed() {
        currentVerticalSpeed = 0f;
    }

    private void UpdateHorizontalSpeed() {
        float deltaSpeed = Mathf.Abs(currentHorizontalSpeed - targetHorizontalSpeed);
        if (deltaSpeed < 0.1f) {
            currentHorizontalSpeed = targetHorizontalSpeed;
            return;
        }

        bool shouldAccelerate = (currentHorizontalSpeed < targetHorizontalSpeed);
        currentHorizontalSpeed += MovementSettings.Acceleration * 
                                  Mathf.Sign(targetHorizontalSpeed - currentHorizontalSpeed) * Time.deltaTime;

        if (shouldAccelerate)
            currentHorizontalSpeed = Mathf.Min(currentHorizontalSpeed, targetHorizontalSpeed);
        else
            currentHorizontalSpeed = Mathf.Max(currentHorizontalSpeed, targetHorizontalSpeed);
    }

    private void ApplyMotion() {
        OrientRotationToMoveVector(MoveVector);

        Vector3 motion = MoveVector * currentHorizontalSpeed + Vector3.up * currentVerticalSpeed;
        controller.Move(motion * Time.deltaTime);
    }

    private bool AlignRotationWithControlRotationY() {
        if (RotationSettings.UseControlRotation) {
            transform.rotation = Quaternion.Euler(0f, ControlRotation.eulerAngles.y, 0f);
            return true;
        }

        return false;
    }

    private bool OrientRotationToMoveVector(Vector3 moveVector){

        if (RotationSettings.OrientRotationToMovement && moveVector.magnitude > 0f){

            Quaternion rotation = Quaternion.LookRotation(moveVector, Vector3.up);
            if (RotationSettings.RotationSmoothing > 0f)
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSettings.RotationSmoothing * Time.deltaTime);
            else
                transform.rotation = rotation;

            return true;
        }

        return false;
    }

    //-------------------------------------------------- for moves
    bool stop = false;
    void CheckDoors(){

        RaycastHit detectedObject;
        if(Physics.Raycast(focusPoint.position, transform.forward, out detectedObject, detectableMasks)){
            if(detectedObject.collider.CompareTag(doorTag)){
                // stop = true;
                MainController.instance.ShowDoorButtonFor(detectedObject.collider.GetComponent<DoorScript>());
            }
            else {
                MainController.instance.HideDoorButton();
            }
        }

    }
}
