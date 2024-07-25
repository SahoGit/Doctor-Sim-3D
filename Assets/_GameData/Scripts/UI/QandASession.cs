using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct QuestionAnswerObject{
    public string question;
    public string[] answers;
    public int rightAnswerIndex;
}

public class QandASession : MonoBehaviour {
    public QuestionAnswerObject[] arrayOfQuestions = new QuestionAnswerObject[0]; //contains data of all questions
    List<QuestionAnswerObject> arrayOfQuestionsToAsk = new List<QuestionAnswerObject>();
    QuestionAnswerObject currentQuestion;//this will hold the current asked question data
    
    [Header("Text Fields")]
    public Text QuestionArea;
    public Text[] arrayOfAnswerTexts = new Text[4];

    public Text resultsTextPanel;

    //for events
    public delegate void OnCompletion();
    public static OnCompletion onCompletion;

    //for the private
    int totalCorrectCount = 0; //will contain the total correct answers by users
    int currentAnswerIndex = 0; //will contain the right answer index
    int currentQuestionNumber = 0; //will contain the current question to be fetched
    
    bool stop = true;
    Animator myAnimator;

    void OnEnable(){
        MainController.instance.ActiveBlackLayer();
    }

    // Start is called before the first frame update
    void Start() {
        myAnimator = GetComponent<Animator>();
        
        if(LevelSelectionScene.missionIndex == 3){
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[5]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[6]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[7]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[8]);
        }
        else if (LevelSelectionScene.missionIndex == 1){
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[0]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[1]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[2]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[3]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[4]);
        }
        else {
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[8]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[5]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[1]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[3]);
            arrayOfQuestionsToAsk.Add(arrayOfQuestions[0]);
        }

        //to load first question
        LoadQuestion();
    }

    void LoadQuestion(){
        currentQuestion = arrayOfQuestionsToAsk[currentQuestionNumber];

        //Loading Question
        QuestionArea.text = currentQuestion.question;       
        //Loading Answer
        arrayOfAnswerTexts[0].text = currentQuestion.answers[0];
        arrayOfAnswerTexts[1].text = currentQuestion.answers[1];
        arrayOfAnswerTexts[2].text = currentQuestion.answers[2];
        arrayOfAnswerTexts[3].text = currentQuestion.answers[3];
        //Loading index
        currentAnswerIndex = currentQuestion.rightAnswerIndex;

        currentQuestionNumber+=1;
        stop = false;
    }

    void DoEndToSession(){
        StartCoroutine(showResults());
    }

    //called with the UI button click
    public void SelectionOptionAt(int index){

        if(!stop){
            stop = true;

            if(index == currentAnswerIndex){
                totalCorrectCount++;
            }
            else{
                Handheld.Vibrate();
            }
            
            if(currentQuestionNumber == arrayOfQuestionsToAsk.Count){
                DoEndToSession();
                return;
            }

            StartCoroutine(loadEffect());
        }
    }
    
    IEnumerator loadEffect(){
        yield return new WaitForSeconds(0.1f);
        
        myAnimator.Play("QuizPanel_QChange");
        yield return new WaitForSeconds(0.5f);

        LoadQuestion();
    }

    IEnumerator showResults(){
        if(totalCorrectCount >= 3)
            resultsTextPanel.text = "You Won. Total Correct Answer is " + totalCorrectCount + " Out of " + arrayOfQuestionsToAsk.Count + ".";
        else
            resultsTextPanel.text = "You Failed!. Total Correct Answer is " + totalCorrectCount + " Out of " + arrayOfQuestionsToAsk.Count +
                                    " Required Minimum 3.";
        yield return new WaitForSeconds(0.1f);
        
        myAnimator.Play("QuizPanel_ShowResults");
        yield return new WaitForSeconds(4.8f);

        if(totalCorrectCount >= 3){
            Debug.Log("You Won!");

            StartCoroutine(EndingSequence());
            if(LevelSelectionScene.missionIndex == 4){
                Gameplay.instance.GameStatus = GameState.task;
            }
        }
        else {
            Debug.Log("You Lose!");

            totalCorrectCount = 0;
            currentQuestionNumber = 0;
            
            StartCoroutine(loadEffect());
        }
    }
    
    IEnumerator EndingSequence(){
        yield return new WaitForSeconds(0.1f);

        // myAnimator.Play("QuizPanel_CLOSING");
        MainController.instance.DisactivateBlackLayer();
        yield return new WaitForSeconds(0.6f);

        if(onCompletion != null){
            onCompletion();
        }
        yield return null;
    }
}
