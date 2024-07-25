using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCanvasController : MonoBehaviour {
    public static EffectCanvasController instance;

    public Animator effectAnimationor;
    
    void Awake(){
        instance = this;
    }

    void Start()
    {
         OpeningEffect();   
    }
    public void OpeningEffect(){
        effectAnimationor.Play("OpeningAnimation");
    }

    public void ClosingEffect(){
        effectAnimationor.Play("ClosingAnimation");
    }
}
