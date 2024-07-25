using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScene : MonoBehaviour
{
    public GameObject Loading;
    public Image LoadingFilled;

    public void LoadingBgActive()
    {
        Loading.SetActive(true);
        StartCoroutine(FillAction(LoadingFilled));
    }

    IEnumerator FillAction(Image img)
    {
        img.fillAmount = 0f;
        while (img.fillAmount < 1)
        {
            img.fillAmount = img.fillAmount + 0.009f;
            yield return new WaitForSeconds(0.03f);
        }
        img.fillAmount = 1f;

        SceneManager.LoadSceneAsync(1);
    }
}
