using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{
    Start,
    Playing,
    Pause,
    task,
    Win,
    Failed
}

public class Gameplay : MonoBehaviour {
    public static Gameplay instance;

    GameState gameStatus;   
    public GameState GameStatus{
        set{
            gameStatus = value;

            if(gameStatus == GameState.Start)
                StartCoroutine(ShowStartingSequence());
            else if(gameStatus == GameState.task)
                StartCoroutine(ShowtaskComplete());
                
            else if(gameStatus == GameState.Win)
                StartCoroutine(ShowLevelWinSequence());

            else if(gameStatus == GameState.Failed)
                StartCoroutine(ShowLevelFailedSequence());
        }
        get{
            return gameStatus;
        }
    }

    void Awake(){
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start() {
        GameStatus = GameState.Start;
    }
    void add(){
        //AssignAdIds_CB.instance.CallInterstitialAd(Adspref.GamePause);
    }

    //-----------------------------------------------------------------
    IEnumerator ShowStartingSequence(){
        Debug.Log("Started..");
        yield return new WaitForSeconds(0.1f);
        
        //for the intro timeline execution
        if(LevelSelectionScene.missionIndex == 0){
            MainController.instance.PlayStartingSequence();
            yield return new WaitForSeconds(10.1f);
        }

        GameObject startingPopup = MainController.instance.StartingPopUp;
        //Invoke("add", 1.0f);
        AdsManager.Instance.ShowInterstitial("Ad show on level starting screen");
        MainController.instance.ShowPopUp(startingPopup);
    }

    IEnumerator ShowLevelWinSequence(){
        Debug.Log("Won..");
        yield return new WaitForSeconds(0.1f);

        GameObject winPopup = MainController.instance.LevelCompletionPopUp;
        // Invoke("add", 1.0f);
        AdsManager.Instance.ShowInterstitial("Ad show on level complete screen");
        MainController.instance.ShowPopUp(winPopup);
        
        //for unlocking the next level
        PlayerPrefs.SetInt("Level" + (LevelSelectionScene.missionIndex + 1) + "Unlocked", 1);
    }
    IEnumerator ShowtaskComplete(){
        Debug.Log("Won..");
        yield return new WaitForSeconds(0.1f);

        GameObject taskPopup = MainController.instance.taskCompletionPopUp;
         //Invoke("add", 1.0f);
         AdsManager.Instance.ShowInterstitial("Ad show on level complete task");
        MainController.instance.ShowPopUp(taskPopup);
    }
    
    IEnumerator ShowLevelFailedSequence(){
        Debug.Log("Failed..");
        yield return new WaitForSeconds(0.1f);
        
        GameObject failedPopup = MainController.instance.LevelFailedPopUp;
        //Invoke("add", 1.0f);
        AdsManager.Instance.ShowInterstitial("Ad show on level failed screen");
        MainController.instance.ShowPopUp(failedPopup);
    }
}
