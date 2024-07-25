using Newtonsoft.Json;
using System;
using System.Collections;
using Unity.RemoteConfig;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class FetchAdsRemoteSettings : MonoBehaviour
{
    struct userAttributes { }
    struct appAttributes { }
    bool hasInitialized = false;

    public void Initialize()
    {
        hasInitialized = true;
        RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void OnFetchCompleted(ConfigResponse response)
    {
        if (response.status == ConfigRequestStatus.Success)
        {
            string json = RemoteConfigService.Instance.appConfig.GetJson("AdsSettings");
            ParseData(json);
        }
    }

    void ParseData(string data)
    {
        if (string.IsNullOrEmpty(data)) return;

        if (data.Length > 2)
            AdsRemoteSettings.Instance.Save(data);
    }

    private void OnDestroy()
    {
        if (hasInitialized)
            RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;
    }
}
