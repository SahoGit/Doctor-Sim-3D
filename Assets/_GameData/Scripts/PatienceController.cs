using UnityEngine;

public class PatienceController : MonoBehaviour {

    public Animator myAnimator;

    // Update is called once per frame
    void OnBecameInvisible () {
        myAnimator.enabled = false;
    }
    
    void OnBecameVisible(){
        myAnimator.enabled = true;
    }
}
