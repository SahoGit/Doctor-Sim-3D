using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SplashScript : MonoBehaviour
{
    public GameObject Loading, Policy,AdsManager;
   
public Image LoadingFilled;
    void Awake()
    {
       
        int temp = PlayerPrefs.GetInt("PolicyLink", 0);
        if (temp == 0)
        {
            Policy.SetActive(true);
        }
        else
        {
           LoadingBgActive();
           
        }
    }

   
    public void Accept()
    {
        PlayerPrefs.SetInt("PolicyLink", 1);
        Policy.SetActive(false);
        LoadingBgActive();
       
    }

    public void Visit()
    {
        Application.OpenURL("https://bestone-games.webnode.com/privacy-policy/");
        //PlayerPrefs.SetInt("PolicyLink", 1);
        //Policy.SetActive(false);
        //LoadingBgActive();
       
    }
   private void LoadingBgActive(){
        AdsManager.SetActive(true);

        Loading.SetActive (true);
		StartCoroutine (FillAction(LoadingFilled));
		Invoke ("LoadingFull", 4.0f);
	}

	IEnumerator FillAction (Image img){
		if (img.fillAmount < 1) {
			img.fillAmount = img.fillAmount + 0.009f;
			yield return new WaitForSeconds (0.02f);
			StartCoroutine (FillAction (img));
		}  else if (img.color.a >= 1f) {
			StopCoroutine (FillAction (img));
		}
	}

	private void LoadingFull(){
		print ("Loading Completed");
		SceneManager.LoadScene(1);
		//NavigationManager.instance.ReplaceScene (GameScene.CLEANINGVIEW);
	}

}
