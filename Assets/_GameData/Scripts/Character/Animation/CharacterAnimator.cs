using UnityEngine;

public class CharacterAnimator : MonoBehaviour {

    public static readonly int HORIZONTAL_SPEED = Animator.StringToHash("HorizontalSpeed");
    public static readonly int VERTICAL_SPEED = Animator.StringToHash("VerticalSpeed");
    public static readonly int IS_GROUNDED = Animator.StringToHash("IsGrounded");
    public static readonly int IDLE = Animator.StringToHash("Idle");
    public static readonly int IDLE_THINKING = Animator.StringToHash("IdleThinking");
    public static readonly int IDLE_REJECTED = Animator.StringToHash("IdleRejected");

    private Animator animator;
    private Character character;
    
    public static bool isCharacterControlsEnabled = true;

    void Awake() {
        for(int i = 0 ; i < 3; i++)
            if(transform.GetChild(i).gameObject.activeSelf)
                animator = transform.GetChild(i).GetComponent<Animator>();

        character = GetComponent<Character>();
    }

    void Update() {
        if(isCharacterControlsEnabled) {
            animator.SetFloat(HORIZONTAL_SPEED, character.HorizontalSpeed);
            animator.SetFloat(VERTICAL_SPEED, character.VerticalSpeed);
        }
        else {
            animator.SetFloat(HORIZONTAL_SPEED, 0);
            animator.SetFloat(VERTICAL_SPEED, 0);
        }
        
        animator.SetBool(IS_GROUNDED, character.IsGrounded);
    }

}
