using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

public class XrayMachine : MonoBehaviour {

    //for the xray area
    public GameObject[] arrayOfXrayImages = new GameObject[0];
    GameObject selectedImage;

    public GameObject issuePointer;
    public GameObject FrameObject;

    public Text messageArea;

    Animator myAnimator;

    public delegate void OnCaptured();
    public static OnCaptured onCaptured;

    void Awake(){
        myAnimator = GetComponent<Animator>();
    }

    void OnEnable(){
        messageArea.text = "Capture the Red Dot inside the yellow box.";
        myAnimator.Play("XrayPanel_Opening");

        //for getting the random image
        selectedImage = arrayOfXrayImages[Random.Range(0, arrayOfXrayImages.Length)];
        selectedImage.SetActive(true);

        //for area range
        Vector3 center = selectedImage.GetComponent<BoxCollider2D>().offset;

        float xRangeR = center.x + (selectedImage.GetComponent<BoxCollider2D>().bounds.size.x / 2f);
        float xRangeL = center.x - (selectedImage.GetComponent<BoxCollider2D>().bounds.size.x / 2f);

        float yRangeT = center.y + (selectedImage.GetComponent<BoxCollider2D>().bounds.size.y / 2f);
        float yRangeB = center.y - (selectedImage.GetComponent<BoxCollider2D>().bounds.size.y / 2f);

        Vector3 placementPos = new Vector3(Random.Range(xRangeL, xRangeR), 64.6f, 0f);
        issuePointer.transform.localPosition = placementPos;

        //for the box movements
        xrangeL = selectedImage.transform.localPosition.x + selectedImage.GetComponent<Image>().sprite.bounds.size.x / 2f * 100;
        xrangeR = selectedImage.transform.localPosition.x - selectedImage.GetComponent<Image>().sprite.bounds.size.x / 2f * 100;
        
        yrangeT = selectedImage.transform.localPosition.y + selectedImage.GetComponent<Image>().sprite.bounds.size.y / 2f * 100;
        yrangeB = selectedImage.transform.localPosition.y - selectedImage.GetComponent<Image>().sprite.bounds.size.y / 2f * 100;

        GoToNextPoint();        
    }
    
    float xrangeL, xrangeR, yrangeT, yrangeB = 0f;
    void GoToNextPoint(){
        FrameObject.transform.DOLocalMove(new Vector3(Random.Range(xrangeL, xrangeR), 64.6f, 0f), 0.4f, false).OnComplete(delegate{
            GoToNextPoint();
        });
    }

    void OnDisable(){
        selectedImage.SetActive(false);
    }

    public void CaptureNow(){
        FrameObject.transform.DOPause();

        if(Vector3.Distance(issuePointer.transform.localPosition, FrameObject.transform.localPosition) < 60){
            messageArea.text = "Captured";

            //if any event registered then calling them
            if(onCaptured != null){
                StartCoroutine(end());
            }
        }
        else {
             messageArea.text = "Not Captured";
             GoToNextPoint();
        }
    }

    IEnumerator end(){
        myAnimator.Play("XrayPanel_Closing");
        yield return new WaitForSeconds(1f);
        
        onCaptured();
        gameObject.SetActive(false);
    }
}
