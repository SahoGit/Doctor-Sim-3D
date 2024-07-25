using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LevelSelectionScene : MonoBehaviour
{
    public static LevelSelectionScene instance;

    public static int missionIndex = 0;

    [Header("Scene Management")]
    public GameObject levelButtonsArea;
    public GameObject[] arrayOfAreaButtons = new GameObject[3];
    public Button[] arrayOfLevelButtons = new Button[0];

    [Header("PopUps")]
    public GameObject RewardedPopUp;
    public GameObject Loading;

    void Awake(){
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start() {
        levelButtonsArea.GetComponent<Animator>().Play("LevelArea_Opening");
        //AssignAdIds_CB.instance.ShowBannerWithPosition(GoogleMobileAds.Api.AdPosition.Bottom);
        AdsManager.Instance.ShowBanner();
        //for unlocking the first level
        PlayerPrefs.SetInt("Level" + 0 + "Unlocked", 1);

        int length = arrayOfLevelButtons.Length;
        for(int i = 0; i < length; i++){
            if(PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 0){
                ColorBlock cb = arrayOfLevelButtons[i].colors;
                cb.normalColor = new Color(1f, 1f, 1f, 0.5f);
                arrayOfLevelButtons[i].colors = cb;
            }
        }
    }

    //------------------------------------------------ for UI
    public void OpenAreaAt(int numberAt){
        levelButtonsArea.GetComponent<Animator>().Play("LevelArea_Button" + (numberAt +1) + "_Opening");
        //if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        //{
        //    AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
        //}
    }

    public void BackToMainArea(){
        Loading.SetActive(true);
        Invoke("mainMenu", 2.0f);
        
    }

    void mainMenu(){
        SceneManager.LoadSceneAsync("1.MainScene");

    }
        

    public void GoToGamePlayScene(int missionNumber){
        missionIndex = missionNumber;

        //if level doesn't unlocked yet
        if (PlayerPrefs.GetInt("UnlockAll", 0) == 1)
        {
            PlayerPrefs.SetInt("Level" + missionIndex + "Unlocked", 1);//making it playable

            //for display
            ColorBlock cb = arrayOfLevelButtons[missionIndex].colors;
            cb.normalColor = new Color(1f, 1f, 1f, 1f);
            arrayOfLevelButtons[missionIndex].colors = cb;
            Loading.SetActive(true);
            Invoke("LoadScene" ,2.0f);

        }
       else if (PlayerPrefs.GetInt("Level" + missionNumber + "Unlocked") == 1){

            // SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
            // LoadingBarScript.instance.LoadNextScene("3.GamePlayScene");
            Loading.SetActive(true);
            Invoke("LoadScene" ,2.0f);
        }
        
        else {
            RewardedPopUp.SetActive(true);
        }
    }

    public void CallRewardedVideoAd(){
        Debug.Log("Rewarded Video Called");
        //------------- YOUR CODE HERE

        //after rewarded video completed, call this method at callback
        RewardedCompleted();
    }

    public void RewardedCompleted(){
        CloseRewardedPopup();

        PlayerPrefs.SetInt("Level" + missionIndex + "Unlocked", 1);//making it playable
        
        //for display
        ColorBlock cb = arrayOfLevelButtons[missionIndex].colors;
        cb.normalColor = new Color(1f, 1f, 1f, 1f);
        arrayOfLevelButtons[missionIndex].colors = cb;
    }
    public void CloseRewardedPopup(){
        RewardedPopUp.SetActive(false);
    }

    //for the back to main menu
    public void LoadScene(){
        SceneManager.LoadSceneAsync("3.GamePlayScene");
        
        //if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        //{
        //    AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
        //}

    }
    public void showad()
    {
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        {
            //AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
            AdsManager.Instance.ShowInterstitial("Ad show remove ads");
        }
    }
}
