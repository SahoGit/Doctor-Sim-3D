using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class PatienceCheckingScene : MonoBehaviour {
    public static PatienceCheckingScene instance;

    public List<GameObject> arrayOfPatiences = new List<GameObject>();
    public GameObject kidPatient;

    GameObject currentPatience;

    [Header("For Pill Pop up")]
    public GameObject pillPopUp;

    [Header("For Path")]
    public Transform[] positionToFollow;
    public Transform sitPoint;
    
    [Header("For UI")]
    public GameObject instructionPanel;

    [Space(10)]
    public Camera myCamera;
    public GameObject[] arrayOfDoctors;
    GameObject currentDoctor;

    public GameObject nurse;

    int counter = 0;
    int numberOfCustomersToServe = 3;

    void Awake() {
        instance = this;
        counter = 0;
    }

    void OnEnable(){
        PillPanelController.onSelectionCompleted += RecommandMedicine;
    }
    
    void OnDisable(){
        PillPanelController.onSelectionCompleted -= RecommandMedicine;
    }
    
    // Start is called before the first frame update
    void Start() {
        //for disabling the controllers
        MainController.instance.DisactiveControls();
        
        //attaching the method
        instructionPanel.transform.Find("ResumeButton").GetComponent<Button>().onClick.AddListener(() => StartPlay());

        if(LevelSelectionScene.missionIndex == 11){
            numberOfCustomersToServe = 1;

            arrayOfPatiences.Clear();
            arrayOfPatiences.Add(kidPatient);
        }
        
        //selecting the doctor
        currentDoctor = arrayOfDoctors[LevelSelectionScene.missionIndex <= 9 ? 0 : 1];
        currentDoctor.SetActive(true);

        //for the character switching
        MainController.instance.mainCharacter.SetActive(false);
        nurse.SetActive(false);

        //camera switching
        myCamera.gameObject.SetActive(true);
        MainController.instance.myCamera.gameObject.SetActive(false);
        
        SetCameraAndDoctor();
    }

    void SetCameraAndDoctor(){
        //for the main doctor movements
        currentDoctor.GetComponent<Animator>().Play("SittindIdle");
        currentDoctor.transform.DOMove(new Vector3(-32.546f, 1.202f, 41.197f), 1f, false);
        currentDoctor.transform.DORotate(new Vector3(0f, 104.8f, 0f), 0.8f).OnComplete(delegate{
            
            if(LevelSelectionScene.missionIndex == 7){
                StartCoroutine(ShowNursePart());
                return;
            }

            MainController.instance.ShowPopUp(instructionPanel);
        });
    }

    IEnumerator ShowNursePart(){
        yield return new WaitForSeconds(0.1f);

        nurse.SetActive(true);
        nurse.transform.position = new Vector3(-27.4f, 1.184f, 43.03f);
        nurse.GetComponent<Animator>().Play("walk");
        nurse.transform.DOMove(new Vector3(-31.932f, 1.248f, 41.997f), 2f, false).OnComplete(delegate{
            nurse.GetComponent<Animator>().Play("talk2");
        });
        
        yield return new WaitForSeconds(5f);
        nurse.GetComponent<Animator>().Play("idle1");
        
        yield return new WaitForSeconds(0.1f);
        MainController.instance.ShowPopUp(instructionPanel);  
    }
    
    //per patience sequence
    public void GetPatience(){
        currentPatience = arrayOfPatiences[Random.Range(0, arrayOfPatiences.Count)];

        currentPatience.transform.position = positionToFollow[0].position;
        currentPatience.SetActive(true);
    }

    IEnumerator BeginAPatience(){
        if(instructionPanel.activeSelf){
            MainController.instance.HidePopUp(instructionPanel, "FailedAndPauseMenu_CLOSING", false);
        }
        
        counter++;
        GetPatience();
        
        currentPatience.GetComponent<Animator>().Play("walk");
        currentPatience.GetComponent<Animator>().speed = 0.8f;
        int length = positionToFollow.Length;
        float step_time = 1.2f;
                
        Sequence sequence = DOTween.Sequence();
        for(int i = 1; i < length; i++){
            sequence.Append(currentPatience.transform.DOMove(positionToFollow[i].position, step_time, false).SetSpeedBased(true));
        }
        sequence.Play();
    
        for(int i = 1; i < length; i++){
            // currentPatience.transform.DOMove(positionToFollow[i].position, step_time, false);
            currentPatience.transform.DOLocalRotateQuaternion(positionToFollow[i].rotation, step_time);
            yield return new WaitForSeconds(step_time);
        }
        
        currentPatience.GetComponent<Animator>().Play("Sitting");
        currentPatience.transform.rotation = sitPoint.rotation;

        Vector3 sitPos = (LevelSelectionScene.missionIndex == 11) ? new Vector3(sitPoint.position.x, 1.226f, sitPoint.position.z) : sitPoint.position;
        currentPatience.transform.DOMove(sitPos, 0.2f, false).OnComplete(delegate{            
            StartCoroutine(ShowPillsPopUps());
        });
    }

    IEnumerator ShowPillsPopUps(){
        yield return new WaitForSeconds(0.5f);
        currentPatience.GetComponent<Animator>().Play("SitTalking");
        
        yield return new WaitForSeconds(2f);
        MainController.instance.ShowPopUp(pillPopUp);
        // pillPopUp.SetActive(true);
    }

    public void RecommandMedicine(){
        StartCoroutine(postSequence());
    }
    IEnumerator postSequence(){
        yield return new WaitForSeconds(0.1f);

        currentPatience.GetComponent<Animator>().Play("SittindIdle");
        currentDoctor.GetComponent<Animator>().Play("Sitting");
        
        yield return new WaitForSeconds(5f);
        currentPatience.GetComponent<Animator>().Play("walk");
        currentPatience.GetComponent<Animator>().speed = 0.8f;

        int length = positionToFollow.Length - 2;
        float step_time = 1.2f;
        Sequence sequence = DOTween.Sequence();
        for(int i = length; i >= 0; i--){
            sequence.Append(currentPatience.transform.DOMove(positionToFollow[i].position, step_time, false).SetSpeedBased(true));
        }
        sequence.Play();
        
        for(int i = length; i >= 0; i--){
            Quaternion des = Quaternion.Euler(180f, positionToFollow[i].rotation.eulerAngles.y, positionToFollow[i].rotation.eulerAngles.z);
            currentPatience.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, des.eulerAngles.y, 0f), step_time);
            yield return new WaitForSeconds(step_time);
        }

        yield return new WaitForSeconds(1.1f);
        currentPatience.SetActive(false);
        arrayOfPatiences.Remove(currentPatience);
        currentPatience = null;
        
        if(counter < numberOfCustomersToServe)
            StartCoroutine(BeginAPatience());
        else{
            Gameplay.instance.GameStatus = GameState.Win;
        }
    }

    //---------------------------------------- from UI
    bool stop = false;
    public void StartPlay(){
        if(!stop){
            stop = true;
            StartCoroutine(BeginAPatience());
        }
    }
}
