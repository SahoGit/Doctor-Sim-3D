using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;
using DG.Tweening;

public class MainController : MonoBehaviour {
    public static MainController instance;
    int targetFrameRate = 60;
    
    public float slowMotion = 0.2f;
    
    [Header("Characters")]
    public GameObject mainCharacter;
    public GameObject[] arrayOfCharacters = new GameObject[0];

    [Header("Level Data")]
    public GameObject[] arrayofLevelData = new GameObject[0];

    [Header("UI")]
    public Image blackTexture;
    public TimerScript timerObject;
    public GameObject doorButton;

    public GameObject smallMessageBox;

    [Space(10)]
    public GameObject HUD;
    public GameObject TouchControllers;
    public Joystick mainJoystick;

    [Space(10)]
    //for the pop ups
    public GameObject StartingPopUp;
    public GameObject PausePopUp;
    public GameObject LevelCompletionPopUp;
    public GameObject taskCompletionPopUp;
    public GameObject LevelFailedPopUp;
    
    [Space(10)]
    public GameObject QuizPopUp;
    public GameObject MedicineInstructionPanel;
    public GameObject pillPrescriptionPopUp;
    
    [Header("Managers")]
    public EffectCanvasController cameraEffectController;


    public GameObject EnvirementObject;

    //--------------------------------- for the local data
    Mission missionData; //for holding the mission to do
    GameObject currentLevelData; //for holding the level data parent gameobject from the scene

    //for holding the current node to go to
    CheckPointData currentPointNode;
    public CheckPointData currentPointToGo {
        get{
            return currentPointNode;
        }
        set {
            currentPointNode = value;
            currentPointNode.gameObject.SetActive(true);
            currentPointNode.checkPointParticle.Play();

            ShowMessageBoxText(currentPointNode.currentMessageBoxDetail);
        }
    }

    //for holding the line renderer used as indicator
    LineRenderer myLineRenderer;
    [HideInInspector]
    public Camera myCamera;

    public GameObject introTimeline;
    public GameObject promotionTimeline;
    public GameObject Loading;

    [Header("Particles System")]
    public ParticleSystem fireworksPS;
    public ParticleSystem heartPS;
    public ParticleSystem heartBrokenPS;
    public ParticleSystem pickupStarPS;
    public ParticleSystem smilyPS;

    void Awake() {
        instance = this;
        // LevelSelectionScene.missionIndex = 1;
       // AssignAdIds_CB.instance.ShowBannerWithPosition(GoogleMobileAds.Api.AdPosition.Top);
        AdsManager.Instance.ShowBanner();
    }
    
    // Start is called before the first frame update
    void Start() {

        EnvirementObject.SetActive(true);

        myCamera = Camera.main;
        myLineRenderer = GetComponent<LineRenderer>();
        //AssignAdIds_CB.instance.ShowBanner();


        //for loading the mission data into the scene
        missionData = (Mission) Resources.Load("Missions/Mission " + LevelSelectionScene.missionIndex);
        
        //for filling the pre-requist scene data
        fillData();

        //for drawing the indicator line
        DrawLineIndicator(currentPointNode.indicatorPointsParentTowardsMe);
    }
    
    //====================================================== For Methods =====================================================
    //for filling the data inside the starting pop up
    void fillData(){
        //starting pop description text filling
        StartingPopUp.transform.Find("DescriptionText").GetComponent<Text>().text = "<b>Mission No. = " + (missionData.missionNumber + 1)
                                                                                    + "</b> \n" + missionData.missionDescription;
        
        //setting timer time
        timerObject.SetTimer(missionData.missionTime);
        if(missionData.isTimeBased){
            timerObject.gameObject.SetActive(true);
        }

        //current level data saving to local variable for easy access
        currentLevelData = arrayofLevelData[missionData.missionNumber];
        currentLevelData.SetActive(true);
        currentPointToGo = currentLevelData.transform.GetChild(0).GetComponent<CheckPointData>(); //saving the first data point;
        
        //activating the particular model of doctor
        int doctorToUse = Mathf.FloorToInt(LevelSelectionScene.missionIndex / 5f);
        doctorToUse = (LevelSelectionScene.missionIndex == 5) ? 0: doctorToUse;
        arrayOfCharacters[doctorToUse].SetActive(true); //activating the doctor
        
        //activating the main character object
        mainCharacter.transform.position = currentLevelData.transform.Find("CharacterStartingPoint").position;
        mainCharacter.transform.rotation = currentLevelData.transform.Find("CharacterStartingPoint").rotation;
        mainCharacter.SetActive(true);
    }

    
    public void DrawLineIndicator(Transform PointsParent){
        myLineRenderer.positionCount = 0; //for the reset

        int length = PointsParent.childCount;
        myLineRenderer.positionCount = length;
        myLineRenderer.SetWidth(0.5f,0.2f);
        for (int i = 0; i < length; i++){
            myLineRenderer.SetPosition(i, PointsParent.GetChild(i).position);
        }
    }

    public void RemoveLineIndicator(){
        myLineRenderer.positionCount = 0; //for the reset
    }

    public void ShowMessageBoxText(string text){
        smallMessageBox.transform.GetChild(0).GetComponent<Text>().text = text;
    }

    void ToggleSlowMotion() {
        Time.timeScale = Time.timeScale == 1f ? this.slowMotion : 1f;
    }

    //========================== For Doors
    DoorScript currentDoor;
    public void ShowDoorButtonFor(DoorScript door){
        currentDoor = door;
        doorButton.SetActive(true);
    }

    public void HideDoorButton(){
        doorButton.SetActive(false);
    }

    public void OpenDoorMethod(){
        currentDoor.DoorTriggered();
    }

    //================================================== For Effect Methods ===================================================
    public void PlayScreenEffect(){
        StartCoroutine(ScreenBlinkEffect());
    }
    IEnumerator ScreenBlinkEffect(){
        cameraEffectController.ClosingEffect();
        yield return new WaitForSeconds(1.5f);
        cameraEffectController.OpeningEffect();
    }
       
    #region ForBlackTexture
    public void ActiveBlackLayer(){
        blackTexture.gameObject.SetActive(true);
        blackTexture.raycastTarget = true;
        blackTexture.DOColor(new Color(0f, 0f, 0f, 180f/255f), 1f);
    }
    public void DisactivateBlackLayer(){
        blackTexture.DOColor(new Color(0f, 0f, 0f, 0f), 1f).OnComplete(delegate{
            blackTexture.raycastTarget = false;
            blackTexture.gameObject.SetActive(false);
        });
    }
    #endregion ForBlackTexture

    #region ForControlsActivaAndDisable
    public void ActiveControls(){
        TouchControllers.SetActive(true);
        mainCharacter.GetComponent<Character>().enabled = true;
    }

    public void DisactiveControls(){
        TouchControllers.SetActive(false);
        mainJoystick.OnPointerUp(null);

        mainCharacter.GetComponent<Character>().enabled = false;
    }
    #endregion ForControlsActivaAndDisable

    #region ForPopUpActivaAndDisable
    //========================== for showing and hiding the pop up
    //this method will take gameobject pop and show it 
    public void ShowPopUp(GameObject popUp){
        SoundManager.instance.PlayEffect(SoundManager.instance.popUpSound);
        popUp.SetActive(true);
        
        ActiveBlackLayer();
        DisactiveControls();
        
    }

    public void HidePopUp(GameObject popup, string AnimaitonName, bool isActiveControl=true){
        StartCoroutine(_HidePopUp(popup, AnimaitonName, isActiveControl));
    }

    IEnumerator _HidePopUp(GameObject popup, string AnimaitonName, bool isActiveControl=true){
        popup.GetComponent<Animator>().Play(AnimaitonName);
        yield return new WaitForSeconds(1f);
        popup.SetActive(false);
        
        DisactivateBlackLayer();

        if(isActiveControl)
            ActiveControls();
    }
    //==========================
    #endregion ForPopUpActivaAndDisable

    //=================================================== For HUD Methods ====================================================
    
    //================================================== For Pop up Methods ==================================================
    //========================== for the starting pop up
    public void StartGame(){
        Gameplay.instance.GameStatus = GameState.Playing;        
        
        //for hiding the pop up
        HidePopUp(StartingPopUp, "FailedAndPauseMenu_CLOSING");
        TimerScript.stopTimer = false;
    }
    
    //========================== for the Pause And Failed Pop up
    public void PauseGame(){
        if(Gameplay.instance.GameStatus != GameState.Playing)
            return;

        //for changing the state
        Gameplay.instance.GameStatus = GameState.Pause;

        //for bring the pop up
        ShowPopUp(PausePopUp);
        //if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        //{
        //    AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
        //}
    }

    public void GoToHomeButton(){
        // AssignAdIds_CB.instance.ShowBannerWithPosition(GoogleMobileAds.Api.AdPosition.Bottom);
        Loading.SetActive(true);
        Invoke("mainmenu", 2.0f);
        
       
    }
    void mainmenu(){
        SceneManager.LoadSceneAsync("1.MainScene", LoadSceneMode.Single);

    }

    public void ResumeGame(){
        

        Gameplay.instance.GameStatus = GameState.Playing;        
        
        //for hiding the pop up
        HidePopUp(PausePopUp, "FailedAndPauseMenu_CLOSING");
    }
    
    public void RestartButton(){
        Loading.SetActive(true);
        Invoke("restart", 2.0f);
        //if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        //{
        //    AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
        //}
    }
    void restart(){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    public void taskCompletionPopUpclose(GameObject parent){
        parent.SetActive(false);
         DisactivateBlackLayer();
        ActiveControls();

    }


    //========================== for the winning pop up
    public void GoToLevelSelectionScene(){
        // AssignAdIds_CB.instance.ShowBannerWithPosition(GoogleMobileAds.Api.AdPosition.Bottom);
        Loading.SetActive(true);
        Invoke("levelSelection", 2.0f);
        
        //if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        //{
        //    AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
        //}

    }
    void levelSelection(){
        SceneManager.LoadSceneAsync("2.SelectionScene", LoadSceneMode.Single);

    }

    //========================== for the levels
    public void PlayStartingSequence(){
        StartCoroutine(firstFunction());
    }

    IEnumerator firstFunction(){
        mainCharacter.SetActive(false);
        myCamera.gameObject.SetActive(false);
        DisactiveControls();
        introTimeline.SetActive(true);

        yield return new WaitForSeconds(10.1f);
        myCamera.gameObject.SetActive(true);
        ActiveControls();
        
        introTimeline.SetActive(false);
        mainCharacter.SetActive(true);
    }
    public void showad()
    {
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        {
           // AssignAdIds_CB.instance.CallInterstitialAd(Adspref.JustStatic);
            AdsManager.Instance.ShowInterstitial("Ad show for removes ads");
        }
    }
}
