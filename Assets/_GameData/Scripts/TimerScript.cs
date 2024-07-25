using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class TimerScript : MonoBehaviour {
    public static TimerScript instance;
    
    public Text timeTextArea;

    float timePerLevel = 300;
    float remainingTime;

    [HideInInspector]
    public static bool stopTimer = true;

    float minutes  = 0;
    float seconds  = 0;

    float totalTime = 0f;

    Animator myAnimator;
    bool isCountDoneStarted = false;

    Gameplay gameplay;

    void Awake(){
        instance = this;
        stopTimer = true;
    }

    // Start is called before the first frame update
    void Start() {
        // Debug.Log("Current Level Time: " + MainController.instance.gameLevel.levelTime);
        // totalTime = 90; //(float) MainController.instance.gameLevel.levelTime;//timePerLevel;    
        // remainingTime = totalTime;    
        gameplay = Gameplay.instance;
        myAnimator = gameObject.GetComponent<Animator>();
        // gameObject.transform.DOShakeRotation(10f, new Vector3(0f, 0f, 90f), 5, 90, false);

        // if (TutorialScript.isTutorial){
        //     this.gameObject.SetActive(false);
        //     stopTimer = true;
        // }
    }

    // Update is called once per frame
    void Update() {
        
        if(!stopTimer && gameplay.GameStatus == GameState.Playing){
            remainingTime -= Time.deltaTime;
            minutes = Mathf.Floor(remainingTime / 60);
            seconds = remainingTime % 60;
            
            if(seconds > 59) seconds = 59;
            
            // if(remainingTime <= 10f && !isCountDoneStarted){
            //     isCountDoneStarted = true;
            //     myAnimator.Play("Timer_Animation");
            // }

            if(minutes < 0) {
                minutes = 0;
                seconds = 0;
                
                Debug.Log("Level Failed");
                stopTimer = true;
                Gameplay.instance.GameStatus = GameState.Failed;
            }
            
            timeTextArea.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    public void SetTimer(int timeInSec){
        timePerLevel = timeInSec;
        totalTime = timeInSec;

        remainingTime = timeInSec;
    }

    public void ResetTimer(){
        remainingTime = timePerLevel;
        stopTimer = false;
    }
}
