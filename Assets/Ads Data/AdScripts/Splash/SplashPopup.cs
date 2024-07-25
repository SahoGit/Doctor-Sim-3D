using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashPopup : MonoBehaviour
{
    [SerializeField] GameObject PolicyPanel;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] Image LoadingBar;
    [SerializeField] [Range(1, 10)] float LoadingDuration = 7f;
    [SerializeField] [Range(0, 5)] int LevelToLoadIndex;

    void Start()
    {
        if (AdConstants.PolicyAccepted)
            OnPolicyAccepted();
        else
        {
            PolicyPanel.SetActive(true);
            LoadingPanel.SetActive(false);
        }
    }

    public void Accept()
    {
        AdConstants.AcceptPolicy();
        OnPolicyAccepted();
    }

    public void VisitWebsite()
    {
        AdsManager.Instance.VisitPrivacyPolicy();
    }

    void OnPolicyAccepted()
    {
        StartCoroutine(LoadNextLevel());

        PolicyPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        AdsManager.Instance.Initialize_AdNetworks();
    }

    IEnumerator LoadNextLevel()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(LevelToLoadIndex);
        async.allowSceneActivation = false;

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / LoadingDuration;
            LoadingBar.fillAmount = timer;
            yield return null;
        }

        async.allowSceneActivation = true;
    }
}
