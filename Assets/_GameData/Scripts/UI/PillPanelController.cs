using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PillPanelController : MonoBehaviour {
    public QuestionAnswerObject[] arrayOfQuestions = new QuestionAnswerObject[0]; //contains data of all questions
    QuestionAnswerObject currentQuestion;//this will hold the current asked question data
    
    [Header("Text Fields")]
    public Text QuestionArea;
    public Text feedbackArea;
    
    [Header("Image Area")]
    public Image pillImage;
    public Sprite[] arrayOfPillImages = new Sprite[3];

    //for the private
    int currentAnswerIndex;
    bool stop = true;
    Animator myAnimator;

    public delegate void OnSelectionCompleted();
    public static OnSelectionCompleted onSelectionCompleted;

    void OnEnable(){
        MainController.instance.ActiveBlackLayer();
        LoadQuestion();
    }

    void Start(){
        myAnimator = GetComponent<Animator>();
    }

    //Method
    void LoadQuestion(){
        feedbackArea.text = "";
        pillImage.enabled = false;

        currentQuestion = arrayOfQuestions[Random.Range(0, arrayOfQuestions.Length)];

        //Loading Question
        QuestionArea.text = currentQuestion.question;
        //Loading index
        currentAnswerIndex = currentQuestion.rightAnswerIndex;
        stop = false;
    }

    //calling from the UI buttons
    public void SelectOptionNumber(int num){
        if(!stop){
            stop = true;

            if(num == currentAnswerIndex){
                feedbackArea.text = "Right Answer!";
            }
            else{
                Handheld.Vibrate();
                feedbackArea.text = "Wrong Answer!";
            }
        }
        
        pillImage.enabled = true;
        pillImage.sprite = arrayOfPillImages[currentAnswerIndex];

        StartCoroutine(EndingSequence());
    }
    
    IEnumerator EndingSequence(){
        yield return new WaitForSeconds(1.0f);
        MainController.instance.HidePopUp(this.gameObject, "FailedAndPauseMenu_CLOSING", false);
        yield return new WaitForSeconds(0.9f);
        
        if(onSelectionCompleted != null)
            onSelectionCompleted();
    }
}