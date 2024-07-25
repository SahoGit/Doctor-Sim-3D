using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestAds : MonoBehaviour
{
    [SerializeField] Text RewardCounter;
    int rewardCount;

    public void ShowConsentForm()
    {
        ConsentManager.GatherConsent(AdsManager.Instance.TestAds, false, (status, message) =>
        {
            ThreadDispatcher.Enqueue(() =>
            {
                MonetizationLogger.Log(message);
            });
        });
    }

    public void ShowInterstitial()
    {
        AdsManager.Instance.ShowInterstitial("TestAds");
    }

    public void ShowRewarded()
    {
        AdsManager.Instance.ShowRewarded(() =>
        {
            rewardCount++;
            RewardCounter.text = "Reward Count : " + rewardCount.ToString();
        }, "TestAds_Count");
    }

    public void BannerShow()
    {
        AdsManager.Instance.ShowBanner();
    }

    public void BannerHide()
    {
        AdsManager.Instance.HideBanner();
    }

    public void BannerDestroy()
    {
        AdsManager.Instance.DestroyBanner();
    }

    public void MrecShow()
    {
        AdsManager.Instance.ShowMREC();
    }

    public void MrecHide()
    {
        AdsManager.Instance.HideMREC();
    }
    public void MrecDestroy()
    {
        AdsManager.Instance.DestroyMREC();
    }

    public void ShowAppOpen()
    {
        AdsManager.Instance.ShowAppOpen();
    }

    public void ShowPrivacyPolicy()
    {
        AdsManager.Instance.VisitPrivacyPolicy();
    }
}
