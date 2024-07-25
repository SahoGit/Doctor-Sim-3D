using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoInternetPopup : MonoBehaviour
{
    [SerializeField] GameObject Popup;
    [SerializeField] bool PauseTimeScale = false;
    [SerializeField] bool MuteAudioListner = false;

    #region OnSceneChanged
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        if (!AdConstants.AdsRemoved)
            CheckInternet();
    }
    #endregion

    #region Popup Visibilty

    void CheckInternet()
    {
        if (Time.realtimeSinceStartup < 3) return;

        if (AdsRemoteSettings.Instance.ShowInternetPopup)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                ShowPopup(true);
                AdsManager.Instance.HideAllBanners();
            }
        }
    }

    public void Retry()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            ShowPopup(false);
            AdsManager.Instance.ResumeAllBanners();
        }
        else
            ShowToast();
    }

    void ShowPopup(bool value)
    {
        Popup.SetActive(value);

        if (PauseTimeScale)
            Time.timeScale = value ? 0 : 1;

        if (MuteAudioListner)
            AudioListener.pause = value;
    }

    public void ShowToast()
    {
        string currentLanguage = Application.systemLanguage.ToString();
        if (currentLanguage.Equals("English"))
            MobileToast.Show("No Internet Connection!", false);
        else
        {
            try
            {
                OpenWifiSettings();
            }
            catch (System.Exception)
            {
                MobileToast.Show("No Internet Connection!", false);
            }
        }
    }

    public void OpenWifiSettings()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass wifiManager = new AndroidJavaClass("android.net.wifi.WifiManager");
        AndroidJavaObject wifiService = context.Call<AndroidJavaObject>("getSystemService", "wifi");

        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
        intent.Call<AndroidJavaObject>("setAction", wifiManager.GetStatic<string>("ACTION_PICK_WIFI_NETWORK"));

        currentActivity.Call("startActivity", intent);
#endif
    }

    #endregion
}