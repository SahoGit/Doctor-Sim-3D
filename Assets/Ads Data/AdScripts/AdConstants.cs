using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdConstants
{
    public static bool InternetAvailable => Application.internetReachability != NetworkReachability.NotReachable;
    public static bool AdsRemoved => PlayerPrefs.GetInt("RemoveAds", 0) == 1;
    public static bool PolicyAccepted => PlayerPrefs.GetInt("PrivacyPolicy", 0) == 1;
    public static bool IsDebugBuild => Debug.isDebugBuild || PlayerPrefs.GetInt("DevMode", 0).Equals(1);

    public static bool ShowAppOpenAfterwards
    {
        get
        {
            if (PlayerPrefs.HasKey("ShowAppOpen"))
                return true;
            else
            {
                PlayerPrefs.SetInt("ShowAppOpen", 1);
                return false;
            }
        }
    }

    public static void AcceptPolicy()
    {
        PlayerPrefs.SetInt("PrivacyPolicy", 1);
        PlayerPrefs.Save();
    }

    public static void SwitchToDebugMode()
    {
        if (IsDebugBuild)
        {
            PlayerPrefs.SetInt("RemoveAds", 1);
            MobileToast.Show("Ads Removed", false);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("DevMode", 1);
            MobileToast.Show("Debug Mode Enabled", false);
            PlayerPrefs.Save();
            Application.Quit();
        }
    }

    public static string GetAdmobTestID(AdFormat format)
    {
#if UNITY_ANDROID
        switch (format)
        {
            case AdFormat.Banner: return "ca-app-pub-3940256099942544/6300978111";
            case AdFormat.MREC: return "ca-app-pub-3940256099942544/6300978111";
            case AdFormat.Interstitial: return "ca-app-pub-3940256099942544/1033173712";
            case AdFormat.Rewarded: return "ca-app-pub-3940256099942544/5224354917";
            case AdFormat.AppOpen: return "ca-app-pub-3940256099942544/9257395921";
            case AdFormat.NativeAd: return "ca-app-pub-3940256099942544/2247696110";
            default: return null;
        }
#elif UNITY_IOS
        switch (format)
        {
            case AdFormat.Banner: return "ca-app-pub-3940256099942544/2934735716";
            case AdFormat.MREC: return "ca-app-pub-3940256099942544/2934735716";
            case AdFormat.Interstitial: return "ca-app-pub-3940256099942544/4411468910";
            case AdFormat.Rewarded: return "ca-app-pub-3940256099942544/1712485313";
            case AdFormat.AppOpen: return "ca-app-pub-3940256099942544/5575463023";
            case AdFormat.NativeAd: return "ca-app-pub-3940256099942544/3986624511";
            default: return null;
        }
#else
        return "unexpected_platform";
#endif
    }
}
