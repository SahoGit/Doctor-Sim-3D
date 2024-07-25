using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

public class MatchingScene : MonoBehaviour {
    
    public Image[] arrayOfQuestions = new Image[0];
    public Image[] arrayOfAnswers = new Image[0];

    public Color CorrectColor;
    public Color IncorrectColor;

    int selectedQuestionNumber = -1;


    int correctAnswers = 0;
    bool stop = false;

    // Start is called before the first frame update
    void OnEnable() {
        MainController.instance.DisactiveControls();
        MainController.instance.ActiveBlackLayer();

        correctAnswers = 0;
    }
    
    // void OnDisable(){
    //     MainController.instance.DisactivateBlackLayer();
    // }

    public void QuestionSelected(int x) {
        if(!stop){
            Reset();

            arrayOfQuestions[x].color = CorrectColor;
            selectedQuestionNumber = x;
        }
    }

    public void AnswerSelected(int x){
        
        if(!stop){
            stop = true;

            if(selectedQuestionNumber == x){
                arrayOfAnswers[x].color = CorrectColor;

                correctAnswers++;
                StartCoroutine(setCorrect(x));

                //winning after after questions are answered.
                if(correctAnswers == arrayOfQuestions.Length){
                    StartCoroutine(madeGameEnd());
                }
            }
            else {
                arrayOfAnswers[x].color = IncorrectColor;
                StartCoroutine(setReset());
            }

            selectedQuestionNumber = -1;
        }
    }

    IEnumerator setCorrect(int x){
        yield return new WaitForSeconds(1f);
        Reset();
        stop = false;

        //for the qa to disable.
        arrayOfQuestions[x].GetComponent<Button>().interactable = false;
        arrayOfAnswers[x].GetComponent<Button>().interactable = false;
    }

    IEnumerator setReset(){
        yield return new WaitForSeconds(0.1f);
        stop = false;
        Reset();
    }
    void Reset(){

        for(int i = 0 ; i < arrayOfQuestions.Length; i++){
            arrayOfQuestions[i].color = Color.white;
            arrayOfAnswers[i].color = Color.white;
        }
    }

    IEnumerator madeGameEnd(){
        MainController.instance.DisactivateBlackLayer();
        int length = gameObject.transform.childCount;
        for(int i = 0 ; i < length; i++)
            gameObject.transform.GetChild(i).gameObject.SetActive(false);

        yield return new WaitForSeconds(01f);
        MainController.instance.promotionTimeline.SetActive(true);
        yield return new WaitForSeconds(10f);

        Gameplay.instance.GameStatus = GameState.Win;
        gameObject.SetActive(false);
    }
}