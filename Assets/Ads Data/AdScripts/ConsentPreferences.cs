using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Ump.Api;

public class ConsentPreferences : MonoBehaviour
{
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] Button SettingsBtn;

    private void Start()
    {
        ShowButton();
    }

    public void OpenSettings()
    {
        LoadingPanel.gameObject.SetActive(true);
        ConsentInformation.Reset();
        ConsentManager.GatherConsent(AdsManager.Instance.TestAds, AdsManager.Instance.IsForFamily, (status, message) =>
        {
            ThreadDispatcher.Enqueue(() =>
            {
                LoadingPanel.gameObject.SetActive(false);
                ShowButton();
            });
        });
    }

    void ShowButton()
    {
        bool UserFromUK = ConsentInformation.ConsentStatus.Equals(ConsentStatus.Obtained);
        SettingsBtn.gameObject.SetActive(UserFromUK);
    }
}