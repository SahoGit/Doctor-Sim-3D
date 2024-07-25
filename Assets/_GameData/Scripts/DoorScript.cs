using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class DoorScript : MonoBehaviour {
    public Vector3 endPosition;
    Vector3 originalPosition;
    Quaternion originalRotation;

    bool isDoorOpened = false;

    public float speed = 1f;
    public bool isRotationBased = false;
    public Vector3 endRotation;
    
    void Start(){
        gameObject.layer = LayerMask.NameToLayer("InteractableItems");
        gameObject.tag = "Door";

        originalPosition = transform.localPosition;
        originalRotation = transform.rotation;
    }
    
    public void DoorTriggered(){

        if(!isDoorOpened) {
            if(!isRotationBased)
                transform.DOLocalMove(endPosition, speed, false);
            else
                transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(endRotation.x, endRotation.y, endRotation.z)), speed);
        }
        else {
            if(!isRotationBased)
                transform.DOLocalMove(originalPosition, speed, false);
            else
                transform.DOLocalRotateQuaternion(originalRotation, speed);
        }

        isDoorOpened = !isDoorOpened;
    }
}
