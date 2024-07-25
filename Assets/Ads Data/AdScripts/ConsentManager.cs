using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsentManager
{
    public static bool IsFetching;
    public static bool ConsentStillRequired => ConsentInformation.ConsentStatus.Equals(ConsentStatus.Unknown);
    public static bool CanShowPersonalizedAds => ConsentInformation.ConsentStatus.Equals(ConsentStatus.NotRequired);

    public static void GatherConsent(bool debugConsent, bool targetUnderAge, Action<ConsentStatus, string> onComplete)
    {
        MonetizationLogger.Log("GatherConsent");
        if (IsFetching)
            return;
        else
        {
            IsFetching = true;
            onComplete += delegate { IsFetching = false; };
        }

        MonetizationLogger.Log("isFetching");

        ConsentRequestParameters requestParameters = new ConsentRequestParameters();
        requestParameters.TagForUnderAgeOfConsent = targetUnderAge;
        if (debugConsent)
            requestParameters.ConsentDebugSettings = GetDebugSettings();

        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            if (updateError != null)
            {
                onComplete(ConsentInformation.ConsentStatus, updateError.Message);
                return;
            }

            if (ConsentInformation.ConsentStatus.Equals(ConsentStatus.Required))
            {
                ConsentForm.Load((ConsentForm form, FormError error) =>
                {
                    if (form != null)
                        ShowConsentForm(form, onComplete);
                    else
                        onComplete(ConsentInformation.ConsentStatus, "Failed to load consent form with error: " + error == null ? "unknown error" : error.Message);
                });
            }
            else
                onComplete(ConsentInformation.ConsentStatus, "Consent has already been gathered or not required.");
        });
    }

    static void ShowConsentForm(ConsentForm _consentForm, Action<ConsentStatus, string> onComplete)
    {
        Time.timeScale = 0;
        _consentForm.Show(
             (FormError error) =>
             {
                 Time.timeScale = 1;
                 if (error == null)
                     onComplete(ConsentInformation.ConsentStatus, "Consent showed successfully");
                 else
                     onComplete(ConsentInformation.ConsentStatus, "Failed to show consent form with error: " + error.Message);
             });
    }

    static ConsentDebugSettings GetDebugSettings()
    {
        List<string> TestDeviceIds = new List<string>()
        {
            AdRequest.TestDeviceSimulator,
#if UNITY_IPHONE
            "96e23e80653bb28980d3f40beb58915c",
#elif UNITY_ANDROID
            "702815ACFC14FF222DA1DC767672A573"
#endif
        };

        return new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.Disabled,
            TestDeviceHashedIds = TestDeviceIds,
        };
    }
}
