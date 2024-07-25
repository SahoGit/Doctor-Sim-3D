using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToNextSceneScript : MonoBehaviour {

	public string SceneName;

	// Use this for initialization
	void Start () {
		GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
	}

	void OnButtonClick(){
		StartCoroutine(goToNextScene());
	}
	
    IEnumerator goToNextScene() {
        Debug.Log(".....................Loading The Desired Scene.....................");
		// SceneLoadFader.instance.BeginFade(1);
		// yield return new WaitForSeconds(SceneLoadFader.instance.fadeSpeed);
		yield return new WaitForSeconds(1f);

		// LoadingScene.LoadScene(SceneNumber);
		SceneManager.LoadSceneAsync (SceneName, LoadSceneMode.Single);
	}
}
