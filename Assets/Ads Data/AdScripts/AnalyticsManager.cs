using System;
using System.Collections;
using GoogleMobileAds.Api;
using System.Text;
using UnityEngine;
using Firebase.Analytics;
using System.Collections.Generic;
//using com.adjust.sdk;

public static class AnalyticsManager
{
    static StringBuilder stringBuilder = new StringBuilder();
    public static string PlacementName;

    #region PaidEvents - Firebase , Adjust
    public static void ReportRevenue_Admob(AdValue admobAd, AdImpressionData data)
    {
        double revenue = (admobAd.Value / 1000000f);
        if (FirebaseManager.hasInitialized)
        {
            var impressionParameters = new[] {
            new Parameter("ad_platform", "Admob"),
            new Parameter("ad_source", "Simple Admob"),
            new Parameter("ad_unit_name", $"{data.Format}({data.Index})_{data.AdUnit}"),
            new Parameter("ad_format", "Admob_" + data.Format.ToString()),
            new Parameter("value", revenue),
            new Parameter("currency", admobAd.CurrencyCode),
            (data.Format == AdFormat.Interstitial || data.Format == AdFormat.Rewarded) ? new Parameter("ad_placement", PlacementName.ToLower()) : new Parameter("no_placement", "nothing")
            };
            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
#if UNITY_IOS
            FirebaseAnalytics.LogEvent("paid_ad_impression", impressionParameters);
#endif
        }

        //Rev Event for Adjust
        //AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
        //adjustAdRevenue.setRevenue(revenue, "USD");
        //Adjust.trackAdRevenue(adjustAdRevenue);

        //Rev Event for Appsflyer
        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("ad_format", "admob_" + data.Format.ToString());
        //AppsFlyerAdRevenue.logAdRevenue("simple_admob", AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob, revenue, "USD", dic);

        //if (data.Format == AdFormat.Interstitial || data.Format == AdFormat.Rewarded)
        //    SendAppsFlyerEvents();
    }

    public static void ReportRevenue_Applovin(MaxSdkBase.AdInfo maxAd, AdFormat format)
    {
        double revenue = maxAd.Revenue;
        if (FirebaseManager.hasInitialized)
        {
            var impressionParameters = new[] {
            new Parameter("ad_platform", "AppLovin"),
            new Parameter("ad_source", maxAd.NetworkName),
            new Parameter("ad_unit_name",$"{format}_{GetMaxNetworkType(maxAd.NetworkName)}_{maxAd.WaterfallInfo.TestName}_{maxAd.AdUnitIdentifier}"),
            new Parameter("ad_format","Applovin_" + format.ToString()),
            new Parameter("value", revenue),
            new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            (format == AdFormat.Interstitial || format == AdFormat.Rewarded) ? new Parameter("ad_placement", PlacementName.ToLower()) : new Parameter("no_placement", "nothing")
            };

            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
#if UNITY_IOS
            FirebaseAnalytics.LogEvent("paid_ad_impression", impressionParameters);
#endif
        }

        //Rev Event for Adjust
        //AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        //adjustAdRevenue.setRevenue(revenue, "USD");
        //adjustAdRevenue.setAdRevenueNetwork(maxAd.NetworkName);
        //adjustAdRevenue.setAdRevenueUnit($"{format}_{maxAd.AdUnitIdentifier}");
        //Adjust.trackAdRevenue(adjustAdRevenue);

        //Rev Event for Appsflyer
        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("ad_unit_name", maxAd.AdUnitIdentifier);
        //dic.Add("ad_format", "applovin_" + format.ToString());
        //AppsFlyerAdRevenue.logAdRevenue(maxAd.NetworkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, revenue, "USD", dic);

        //if (format == AdFormat.Interstitial || format == AdFormat.Rewarded)
        //    SendAppsFlyerEvents();
    }

    static string GetMaxNetworkType(string NetworkName)
    {
        NetworkName = NetworkName.ToLower();
        switch (NetworkName)
        {
            case "google admob native":
            case "ironsource":
            case "unity ads":
            case "google ad manager":
            case "google ad manager native":
            case "dt exchange":
                return "Waterfall";

            case "google admob":
            case "mintegral":
            case "applovin":
            case "pangle":
            case "applovin_exchange":
            case "liftoff monetize":
            case "inmobi":
            case "smaato":
            case "smaato native":
            case "tapjoy":
            case "adcolony":
            case "facebook":
                return "Bidding";

            default:
                return NetworkName;
        }
    }

    #endregion

    #region PlacementEvents - AppMetrica

    public static void ReportPlacementEvent(AdNetwork network, AdFormat format)
    {
        string jsonData = $"{{\"{format}\":\"{PlacementName}\"}}";
        AppMetrica.Instance.ReportEvent("Placements", jsonData);
    }

    #endregion

    #region CustomEvents - Appmetrica

    public static void ReportCustomEvent(AnalyticsType type, params string[] data)
    {
        if (!data.IsNullOrEmpty())
        {
            ThreadDispatcher.Enqueue(() =>
            {
                AppMetrica.Instance.ReportEvent(type.ToString(), GetJsonFromParams(data));
            });
        }
    }

    #endregion

    #region Extensions
    static bool IsNullOrEmpty(this string[] value)
    {
        if (value != null)
        {
            return value.Length == 0;
        }
        return true;
    }

    static string GetJsonFromParams(params string[] data)
    {
        stringBuilder.Clear();
        int dataLength = data.Length;
        for (int i = 0; i < dataLength; i++)
        {
            if (i == dataLength - 1)
            {
                stringBuilder.Append($"\"{data[i]}\"");
                break;
            }
            stringBuilder.Append($"{{\"{data[i]}\":");
        }
        stringBuilder.Append('}', data.Length - 1);
        return stringBuilder.ToString();
    }
    #endregion

    #region Appsflyer
    //static int ImpressionCount
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt("ImpressionCount", 0);
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt("ImpressionCount", value);
    //    }
    //}


    //static TimeSpan TimeSpan()
    //{
    //    DateTime FirstOpen, CurrentDate;
    //    if (!PlayerPrefs.HasKey("FirstOpenDate"))
    //        PlayerPrefs.SetString("FirstOpenDate", DateTime.Now.ToBinary().ToString());

    //    long temp = Convert.ToInt64(PlayerPrefs.GetString("FirstOpenDate"));

    //    FirstOpen = DateTime.FromBinary(temp);
    //    CurrentDate = System.DateTime.Now;
    //    return CurrentDate.Subtract(FirstOpen);
    //}
    //static void SendAppsFlyerEvents()
    //{
    //    if (TimeSpan().TotalDays < 1)
    //    {
    //        ImpressionCount++;
    //        if (ImpressionCount > 5 && ImpressionCount <= 20)
    //            AppsFlyer.sendEvent("af_ad_watched_x" + ImpressionCount.ToString(), null);
    //    }
    //}
    #endregion
}

public enum AnalyticsType { Extras, GameData }
public enum AdNetwork { Admob, Applovin, ironSource }
public enum AdFormat { Banner, MREC, Interstitial, Rewarded, AppOpen, NativeAd, RewardedInt }