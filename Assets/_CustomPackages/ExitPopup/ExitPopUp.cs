using System.Collections;
using UnityEngine;

public class ExitPopUp : MonoBehaviour {

    [Header("UI Elements")]
    public GameObject blackTexture;
    public GameObject exitPopUp;
    
    public GameObject[] arrayOfButtonsToHide = new GameObject[0];

    AudioSource myAudioSource;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start() {
        myAudioSource = this.GetComponent<AudioSource>();
        myAnimator = exitPopUp.GetComponent<Animator>();

        //
        blackTexture.SetActive(false);
        exitPopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
        if(Input.GetKeyDown(KeyCode.Escape) && !exitPopUp.activeSelf){
            blackTexture.SetActive(true);

            exitPopUp.SetActive(true);
            myAnimator.Play("ExitPopUp_Opening");

            //for disabling the buttons
            int length = arrayOfButtonsToHide.Length;
            for(int i = 0 ; i < length; i++)
                arrayOfButtonsToHide[i].SetActive(false);
        }
    }

    //========================================== For UI Element
    public void YesButtonClicked(){
        myAudioSource.Play();

        Application.Quit();
    }

    public void NoButtonClicked(){
        myAudioSource.Play();

        StartCoroutine(ExitSequence());
    }

    IEnumerator ExitSequence(){
        myAnimator.Play("ExitPopUp_Closing");

        yield return new WaitForSeconds(0.9f);
        blackTexture.SetActive(false);
        exitPopUp.SetActive(false);

        //for enabling the buttons
        int length = arrayOfButtonsToHide.Length;
        for(int i = 0 ; i < length; i++)
            arrayOfButtonsToHide[i].SetActive(true);
    }
}