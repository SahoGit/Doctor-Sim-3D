using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainScene : MonoBehaviour {

    public GameObject bgMusic;
    public static GameObject bgMusicInstance;
    public GameObject Loading;

    void Awake(){

        if(bgMusicInstance != null){
            DestroyImmediate(bgMusic);
        }
        else {
            bgMusicInstance = bgMusic;
            DontDestroyOnLoad(bgMusicInstance);
        }
        //AssignAdIds_CB.instance.ShowBannerWithPosition(GoogleMobileAds.Api.AdPosition.Bottom);
        AdsManager.Instance.ShowBanner();
    }
    
    // =============================== for the privacy policy
    public GameObject privacyPopUp;
    public string privacyPolicyLink;
    public void OpenPrivacyPopUp(){
        Application.OpenURL(privacyPolicyLink);
    }

    public void ClosePrivacyPopup(){
        privacyPopUp.SetActive(false);
    }

    public void OpenPrivacyLink(){
        Application.OpenURL(privacyPolicyLink);
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=4826365601331502275");
    }

    public void RateGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.doctor.simulator.medical.surgeon.dr.hospital.games");
    }
    // =============================== for play button
    public void PlayGame(){
        Loading.SetActive(true);
        Invoke("play" ,2.0f);
        
    }
    void play(){
         SceneManager.LoadSceneAsync("2.SelectionScene");
        
    }
}
