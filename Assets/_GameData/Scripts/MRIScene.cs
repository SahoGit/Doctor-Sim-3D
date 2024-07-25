using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class MRIScene : MonoBehaviour {
    public static MRIScene instance;
    
    [Header("For Patience")]
    public List<GameObject> arrayOfPatiences = new List<GameObject>();
    GameObject currentPatience;

    [Space(10)]
    public Transform mri_Bad;
    public GameObject lightObject;
    public float speed = 0;

    [Header("For Path")]
    public Transform[] positionToFollow;
    public Transform sitPoint;

    [Header("For UI")]
    public GameObject mri_UI_Panel;
    Button machineButton;
    public Transform BarPanel;

    GameObject reportPopUp;

    Vector3 originalPosition;
    public Vector3 endPosition;

    Vector3 desPosition;

    bool startMRI = false;
    int counter = 0;
    int patienceCounter = 0;
    int numberOfPatientToComplete = 3;

    //for events
    public delegate void OnCompletion();
    public static OnCompletion onCompletion;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }

    void OnEnable(){
        originalPosition = mri_Bad.localPosition;
        desPosition = endPosition;
        MainController.instance.mainCharacter.SetActive(false);
        
        counter = 0;
        patienceCounter = 0;

        mri_UI_Panel.SetActive(true);
    }

    void Start() {
        //for disabling the controllers
        MainController.instance.DisactiveControls();
        machineButton = mri_UI_Panel.transform.Find("MRIButton").GetComponent<Button>();
        reportPopUp = mri_UI_Panel.transform.Find("ReportPanel").gameObject;

        machineButton.interactable = false;
        
        if(LevelSelectionScene.missionIndex == 12)
            numberOfPatientToComplete = 4;

        //camera switching
        MainController.instance.myCamera.gameObject.SetActive(false);

        StartCoroutine(BeginAPatience());
    }

    // Update is called once per frame
    void Update() {

        if(startMRI){
            mri_Bad.localPosition = Vector3.MoveTowards(mri_Bad.localPosition, desPosition, speed * Time.deltaTime);

            if(Vector3.Distance(mri_Bad.localPosition, desPosition) <= 0.1){
                counter++;
                BarPanel.localScale = new Vector3(BarPanel.localScale.x + 0.25f, 1, 1);
                
                desPosition = (counter % 2 == 1) ? originalPosition : endPosition;

                if (counter % 4 == 0){
                    startMRI = false;
                    lightObject.SetActive(false);
                    
                    StartCoroutine(postSequence());
                }
            }
        }
    }

    
    //per patience sequence
    public void GetPatience(){
        currentPatience = arrayOfPatiences[Random.Range(0, arrayOfPatiences.Count)];

        currentPatience.transform.position = positionToFollow[0].position;
        currentPatience.SetActive(true);
    }

    IEnumerator BeginAPatience(){
        GetPatience();
        
        currentPatience.GetComponent<Animator>().Play("walk");
        int length = positionToFollow.Length;
        for(int i = 1; i < length; i++){
            currentPatience.transform.DOMove(positionToFollow[i].position, 2f, false);
            currentPatience.transform.DOLocalRotateQuaternion(positionToFollow[i].rotation, 1f);
            yield return new WaitForSeconds(1.8f);
        }

        currentPatience.GetComponent<Animator>().Play("idle1");
        currentPatience.transform.parent = mri_Bad;
        currentPatience.transform.rotation = sitPoint.rotation;
        currentPatience.transform.DOMove(sitPoint.position, 0.2f, false).OnComplete(delegate{
            lightObject.SetActive(true);
            machineButton.interactable = true;
            BarPanel.parent.gameObject.SetActive(true);
        });
    }

    IEnumerator postSequence(){
        machineButton.interactable = false;
        EndMachine();
        yield return new WaitForSeconds(0.1f);

        BarPanel.parent.gameObject.SetActive(false);
        BarPanel.localScale = new Vector3(0f, 1, 1);
        
        //for the pop up to show up
        reportPopUp.transform.Find("DescriptionText").GetComponent<Text>().text = Random.Range(0, 2) == 0 ? 
                                                                    "Your MRI goes sucessfully. No problem detected." :
                                                                    "Your MRI goes un-sucessfully. We need to do further testing on you. Go to Doctor's room.";
        MainController.instance.ShowPopUp(reportPopUp);
    }
    
    //===================================================== for ui
    public void StartMachine(){
        if(machineButton.interactable)
            startMRI = true;
    }

    public void EndMachine(){
        startMRI = false;
    }

    public void HideReport(){
        StartCoroutine(SendCurrentPatientToHome());
    }

    IEnumerator SendCurrentPatientToHome(){
         MainController.instance.HidePopUp(reportPopUp, "FailedAndPauseMenu_CLOSING", false);
        yield return new WaitForSeconds(1f);
        
        currentPatience.GetComponent<Animator>().Play("walk");
        
        Transform lastPoint = positionToFollow[positionToFollow.Length - 1];
        currentPatience.transform.position = lastPoint.position;
        currentPatience.transform.rotation = Quaternion.Euler(new Vector3(180f, lastPoint.rotation.eulerAngles.y, 90f));
        
        int length = positionToFollow.Length;
        for(int i = length - 2; i >= 0; i--){
            currentPatience.transform.DOMove(positionToFollow[i].position, 1.5f, false);
            Quaternion des = Quaternion.Euler(180f, positionToFollow[i].rotation.eulerAngles.y, 90);
            currentPatience.transform.DOLocalRotateQuaternion(Quaternion.Euler(180f, des.eulerAngles.y, 90f), 0.1f);
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(1.1f);
        currentPatience.SetActive(false);
        
        patienceCounter++;
        if(patienceCounter < numberOfPatientToComplete){
            arrayOfPatiences.Remove(currentPatience);
            currentPatience = null;
            
            StartCoroutine(BeginAPatience());
        }
            
        else{
            machineButton.gameObject.SetActive(false);
            
            if(onCompletion != null){
                Gameplay.instance.GameStatus = GameState.task;
                onCompletion();
            }
            else
                Gameplay.instance.GameStatus = GameState.Win;
        }
    }
}
