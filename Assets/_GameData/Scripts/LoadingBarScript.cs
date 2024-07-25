// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// using System.Collections;

// public class LoadingBarScript : MonoBehaviour {

//     public static LoadingBarScript instance;

//     public Text loadingText;
//     public Image sliderBar;

//     void Awake() {
//         instance = this;
//     }
//     void start(){
//         Invoke ("add",1.0f);
//     }

//     void add(){
//         AssignAdIds_CB.instance.CallInterstitialAd(Adspref.GamePause);
//     }

//     public void LoadNextScene(string LoadingSceneName){
//         int length = transform.childCount;
//         for (int i = 0; i < length; i++)
//         {
//             transform.GetChild(i).gameObject.SetActive(true);
//         }

//         // ...change the instruction text to read "Loading..."
//         loadingText.text = "Loading...";

//         // ...and start a coroutine that will load the desired scene.
//         StartCoroutine(LoadNewScene(LoadingSceneName));
//     }

//      // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
//     IEnumerator LoadNewScene(string sceneName) {
//         yield return new WaitForSeconds(2.0f);
//         // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
//         AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
//         // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
//         while (!async.isDone) {

//             float progress = Mathf.Clamp01(async.progress / 0.9f);
//             yield return new WaitForSeconds(2.0f);
            
//             sliderBar.fillAmount = progress;
//             loadingText.text = ((int)(progress * 100)) + "%";
//             yield return null;
//         }
//     }

// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarScript  : MonoBehaviour
{
    public Image Loading;
    float i;
   
    void Start()
    {
        Loading.fillAmount = 0;
        Invoke("add", 1.0f); 
        
    }
    void add(){
       // AssignAdIds_CB.instance.CallInterstitialAd(Adspref.GamePause);
        //AdsManager.Instance.ShowInterstitial("Ad show on loading screen");
    }


    void Update()
    {
        i += (float)0.7 * Time.deltaTime;
        Loading.fillAmount = i;
        
    }
}
