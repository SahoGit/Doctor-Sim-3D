using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBannerActivator : MonoBehaviour
{
   // public BannerStatus Enable = BannerStatus.Show;
   // public BannerStatus Disable = BannerStatus.Hide;
   // public AdPosition NewBannerPosition = AdPosition.Top;

    public  void OnEnable()
    {

        AdsManager.Instance.ShowMREC();
        /* if(Enable== BannerStatus.Show)
        { 
            BigBannerHandler.instance.ShowBigBanner(NewBannerPosition);
            
        }
        else
        {
           BigBannerHandler.instance.HideBigBanner();
        } */
    }

    public  void OnDisable()
    {

     AdsManager.Instance.HideMREC();
       /*  if (Disable != BannerStatus.Hide)
        {
            BigBannerHandler.instance.ShowBigBanner(NewBannerPosition);
        }
        else
        {
           BigBannerHandler.instance.HideBigBanner();
        } */
    }
    // public enum BannerStatus
    // {
    //     Hide,Show
    // }
}
