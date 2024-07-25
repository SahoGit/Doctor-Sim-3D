using UnityEngine;

public static class InputController {
    static float lookAngle = 180f;
    static float tiltAngle = 0f;

    public static Vector3 GetMovementInput(Camera relativeCamera) {
        Vector3 moveVector;
        float horizontalAxis = Character.myJoystick.Horizontal; //Input.GetAxis("Horizontal");
        float verticalAxis = Character.myJoystick.Vertical; //Input.GetAxis("Vertical");

        if (relativeCamera != null) {
            // Calculate the move vector relative to camera rotation
            Vector3 scalerVector = new Vector3(1f, 0f, 1f);
            Vector3 cameraForward = Vector3.Scale(relativeCamera.transform.forward, scalerVector).normalized;
            Vector3 cameraRight = Vector3.Scale(relativeCamera.transform.right, scalerVector).normalized;

            moveVector = (cameraForward * verticalAxis + cameraRight * horizontalAxis);
        }
        else {
            // Use world relative directions
            moveVector = (Vector3.forward * verticalAxis + Vector3.right * horizontalAxis);
        }

        if (moveVector.magnitude > 1f)
            moveVector.Normalize();

        return moveVector;
    }
    
    //for getting the camera rotation angle in the response of input
    public static Quaternion GetMouseRotationInput(float mouseSensitivity = 0.07f, float minTiltAngle = -75f, float maxTiltAngle = 45f) {
        float mouseX = RotationPanel.TouchDist.x; //Input.GetAxis("Mouse X");
        float mouseY = RotationPanel.TouchDist.y; //Input.GetAxis("Mouse Y");

        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += mouseY * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        return Quaternion.Euler(-tiltAngle, lookAngle, 0f);
    }

    public static Quaternion GetControllerRotationInput(float mouseSensitivity = 2f, float minTiltAngle = -75f, float maxTiltAngle = 45f) {
        float mouseX = Character.myJoystick.Horizontal; //Input.GetAxis("Mouse X");
        float mouseY = Character.myJoystick.Vertical; //Input.GetAxis("Mouse Y");
        
        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += mouseY * mouseSensitivity * 0;
        tiltAngle %= 360f;
        tiltAngle = ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        return Quaternion.Euler(-tiltAngle, lookAngle, 0f);
    }

    //for getting the camera rotation angle in the response of input
    public static Quaternion GetMouseRotationAuto(float mouseSensitivity = 2f, float minTiltAngle = -75f, float maxTiltAngle = 45f) {
        float mouseX = Character.myJoystick.Horizontal; 
        
        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += 0f * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        return Quaternion.Euler(tiltAngle, lookAngle, 0f);
    }

    static float ClampAngle(float angle, float min, float max) {
        while (angle < -360f || angle > 360f) {

            if (angle < -360f)
                angle += 360f;
            else if (angle > 360f)
                angle -= 360f;
        }
        
        return Mathf.Clamp(angle, min, max);
    }

    public static bool GetSprintInput() {
        return Input.GetButton("Sprint");
    }

    public static bool GetJumpInput() {
        return Input.GetButtonDown("Jump");
    }

    public static bool GetToggleWalkInput() {
        return Input.GetButtonDown("Toggle Walk");
    }
}
