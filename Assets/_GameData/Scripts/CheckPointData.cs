using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointData : MonoBehaviour {

    [Header("Message")]
    public string currentMessageBoxDetail = "";

    [Space(10)]
    public CheckPointData nextPointToGo;
    public Transform indicatorPointsParentTowardsMe;

    public bool isLastPoint = false;
    public bool isExtraEnd = false;
    public GameObject[] objectsToActiveAtEnd = new GameObject[0];
    public GameObject[] objectsToDisable = new GameObject[0];
  

    [Header("CutScene")]
    public GameObject cutSceneObject;
    public float timelineClipTiming = 4f;
    public bool wantToPlayCutSceneAtEnd = true;
    
    [Header("Particles And Sound Objects")]
    public ParticleSystem checkPointParticle;
    
    //for private
    bool stop = false;
    
    // void Awake(){
    //     checkPointParticle.Stop();
    // }

    // Start is called before the first frame update
    void OnEnable() {
        if(LevelSelectionScene.missionIndex == 2){
            XrayMachine.onCaptured += Level2;
        }
        if(LevelSelectionScene.missionIndex == 1 || LevelSelectionScene.missionIndex == 3){
            QandASession.onCompletion += Level1;
        }
        if(LevelSelectionScene.missionIndex == 4){
            QandASession.onCompletion += Level3;
            XrayMachine.onCaptured += Level4;
        }
        //for the level 8
        if(LevelSelectionScene.missionIndex == 7 || LevelSelectionScene.missionIndex == 13){
            PillPanelController.onSelectionCompleted += PostLevel8Sequence;
        }
        
        //for the level 13
        if(LevelSelectionScene.missionIndex == 12 && nextPointToGo != null)
            MRIScene.onCompletion += OnMRICompleted;
        else if(LevelSelectionScene.missionIndex == 12 && nextPointToGo == null)
            XrayMachine.onCaptured += OnXrayPart;
    }
    
    void OnDisable(){

        if(LevelSelectionScene.missionIndex == 2){
            XrayMachine.onCaptured -= Level2;
        }
        if(LevelSelectionScene.missionIndex == 1 || LevelSelectionScene.missionIndex == 3){
            QandASession.onCompletion -= Level1;
        }
        if(LevelSelectionScene.missionIndex == 4){
            QandASession.onCompletion -= Level3;
            XrayMachine.onCaptured -= Level4;
        }

        //for the level 12
        if(LevelSelectionScene.missionIndex == 12 && nextPointToGo != null)
            MRIScene.onCompletion -= OnMRICompleted;
        else if(LevelSelectionScene.missionIndex == 12 && nextPointToGo == null)
            XrayMachine.onCaptured -= OnXrayPart;
    }

    public void OnTriggerEnter (Collider other){

        if(other.gameObject.tag == "Player" && !stop){
            Debug.Log("Player Collided.");

            if(MainController.instance.currentPointToGo.GetInstanceID() == this.GetInstanceID()){
                stop = true;
                
                MainController.instance.DisactiveControls();
                other.gameObject.transform.position = this.transform.position;
                
                SoundManager.instance.PlayEffect(SoundManager.instance.checkPointSound); //for playing the sound
                checkPointParticle.Stop(); //for stopping the effect

                StartCoroutine(ControlBankToMainCamera());
            }
        }
    }
    
    IEnumerator ControlBankToMainCamera(){
        EffectCanvasController cameraEffectController = MainController.instance.cameraEffectController;
        cameraEffectController.ClosingEffect();
        yield return new WaitForSeconds(1f);

        //for the cut scene sequence
        if(cutSceneObject != null && wantToPlayCutSceneAtEnd){
            cameraEffectController.OpeningEffect();
            yield return new WaitForSeconds(0.5f);
            MainController.instance.myCamera.enabled = false;        
            cutSceneObject.SetActive(true);
            
            yield return new WaitForSeconds(timelineClipTiming);//cut scene time

            cameraEffectController.ClosingEffect();
            yield return new WaitForSeconds(1f);

            MainController.instance.myCamera.enabled = true;
            cutSceneObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);
        cameraEffectController.OpeningEffect();
        
        if(!isLastPoint){
            if(!isExtraEnd){
                Gameplay.instance.GameStatus = GameState.task;
            }
            if(LevelSelectionScene.missionIndex == 4)
                StartCoroutine(Level2Sequence());
            else if (LevelSelectionScene.missionIndex == 12){
                //for enabling and disabling the objects
                OnEnableDisable();
            }
            else if (LevelSelectionScene.missionIndex == 7 || LevelSelectionScene.missionIndex == 13){
                StartingPointForLevel8();
            }
            else {
                SetTheNextPoint();
            }
        }
        
        else {
            //if it is the last node, then disable messsage box
            MainController.instance.smallMessageBox.SetActive(false);

            if(!isExtraEnd)
                Gameplay.instance.GameStatus = GameState.Win;

            else {
                if(LevelSelectionScene.missionIndex == 1)
                    StartCoroutine(Level2Sequence());
                else if(LevelSelectionScene.missionIndex == 3)
                    StartCoroutine(Level2Sequence());

                else if(LevelSelectionScene.missionIndex == 6)
                    StartCoroutine(Level6Sequence());
                
                else if (LevelSelectionScene.missionIndex == 7 || LevelSelectionScene.missionIndex == 13){
                    StartingPointForLevel8();
                }                
            }

            //for enabling and disabling the objects
            OnEnableDisable();
            //drawing the indicator
            MainController.instance.RemoveLineIndicator();
            yield return null;
        }
    }

    void SetTheNextPoint(){
        MainController.instance.currentPointToGo = nextPointToGo; //saving the next point address
        MainController.instance.ActiveControls();
        
        //drawing the indicator
        MainController.instance.DrawLineIndicator(MainController.instance.currentPointToGo.indicatorPointsParentTowardsMe);
        this.gameObject.SetActive(false);
    }
    void OnEnableDisable(){

        for(int i = 0 ;i < objectsToActiveAtEnd.Length; i++){
            objectsToActiveAtEnd[i].SetActive(true);
        }
        for(int i = 0 ;i < objectsToDisable.Length; i++){
            objectsToDisable[i].SetActive(false);
        }
    }

    //=---------------- Level 2 Sequence;
    IEnumerator Level2Sequence(){
        EffectCanvasController cameraEffectController = MainController.instance.cameraEffectController;
        cameraEffectController.ClosingEffect();
        yield return new WaitForSeconds(1f);

        cameraEffectController.OpeningEffect();
        yield return new WaitForSeconds(0.5f);
        MainController.instance.myCamera.enabled = false;        
        cutSceneObject.SetActive(true);
        
        yield return new WaitForSeconds(timelineClipTiming);//cut scene time
        
        MainController.instance.QuizPopUp.SetActive(true);
    }
    
    //for the level 2 callback (Q and A)
    void Level1(){
        QandASession.onCompletion -= Level1;

        Gameplay.instance.GameStatus = GameState.Win;
        MainController.instance.QuizPopUp.SetActive(false);
    }

    //============================= for level 3 - sequence after xray
    void Level2(){
        Debug.Log("Level 3 Ending.");
        XrayMachine.onCaptured -= Level2;

        Gameplay.instance.GameStatus = GameState.Win;
    }
    
    //============================= for level 4 - sequence after question and answer
    void Level3(){
        QandASession.onCompletion -= Level3;
        
        MainController.instance.myCamera.enabled = true;
        cutSceneObject.SetActive(false);
        MainController.instance.QuizPopUp.SetActive(false);

        MainController.instance.DisactivateBlackLayer();
        MainController.instance.ActiveControls();

        MainController.instance.currentPointToGo = nextPointToGo; //saving the next point address
        //drawing the indicator
        MainController.instance.DrawLineIndicator(MainController.instance.currentPointToGo.indicatorPointsParentTowardsMe);
    }
    
    //============================= for level 4 - sequence after xray
    void Level4(){
        Debug.Log(" ================== Level 5 Ending. ================== ");
        XrayMachine.onCaptured -= Level4;
        stop = true;
        MainController.instance.DisactiveControls();
        
        StartCoroutine(end4());
    }
    IEnumerator end4(){
        EffectCanvasController cameraEffectController = MainController.instance.cameraEffectController;
        cameraEffectController.ClosingEffect();
        yield return new WaitForSeconds(1f);

        //for the cut scene sequence
        cameraEffectController.OpeningEffect();
        yield return new WaitForSeconds(0.5f);
        MainController.instance.myCamera.enabled = false; 
        cutSceneObject.SetActive(true);
        
        yield return new WaitForSeconds(timelineClipTiming);//cut scene time
        Gameplay.instance.GameStatus = GameState.Win;
    }
    
    //============================= for level 7
    IEnumerator Level6Sequence(){
        EffectCanvasController cameraEffectController = MainController.instance.cameraEffectController;
        cameraEffectController.ClosingEffect();
        yield return new WaitForSeconds(1f);
        
        cameraEffectController.OpeningEffect();
    }
    
    //============================= for level 8
    static bool instructionPanelShowed = false;
    public void StartingPointForLevel8(){
        if(!instructionPanelShowed){
            instructionPanelShowed = true;

            //attaching the method
            GameObject insPop = MainController.instance.MedicineInstructionPanel;
            insPop.transform.Find("ResumeButton").GetComponent<Button>().onClick.AddListener(() => HideInstructionPanel());
            
            MainController.instance.ShowPopUp(insPop);
            return;
        }

        StartCoroutine(Level8Starting());
    }

    void HideInstructionPanel() {
        GameObject insPop = MainController.instance.MedicineInstructionPanel;
        insPop.transform.Find("ResumeButton").GetComponent<Button>().onClick.RemoveAllListeners();
        
        MainController.instance.HidePopUp(insPop, "FailedAndPauseMenu_CLOSING", false);
        StartCoroutine(Level8Starting());
    }

    IEnumerator Level8Starting(){
        yield return new WaitForSeconds(1f);
        objectsToActiveAtEnd[0].GetComponent<Animator>().Play("talk1");
        
        yield return new WaitForSeconds(2f);
        MainController.instance.ShowPopUp(MainController.instance.pillPrescriptionPopUp);
    }

    void PostLevel8Sequence(){
        PillPanelController.onSelectionCompleted -= this.PostLevel8Sequence;

        if(nextPointToGo != null)
            SetTheNextPoint(); //for going on the next point
        else 
            Gameplay.instance.GameStatus = GameState.Win;
    }

    //============================= for level 13
    //for the MRI part
    public void OnMRICompleted(){
        MainController.instance.myCamera.gameObject.SetActive (true);
        MainController.instance.mainCharacter.SetActive(true);
        
        MRIScene.instance.gameObject.SetActive(false);

        MainController.instance.DisactivateBlackLayer();
        MainController.instance.ActiveControls();

        MainController.instance.currentPointToGo = nextPointToGo; //saving the next point address
        //drawing the indicator
        MainController.instance.DrawLineIndicator(MainController.instance.currentPointToGo.indicatorPointsParentTowardsMe);
    }
    
    public void OnXrayPart(){
        XrayMachine.onCaptured -= OnXrayPart;
        Gameplay.instance.GameStatus = GameState.Win;
    }
}
