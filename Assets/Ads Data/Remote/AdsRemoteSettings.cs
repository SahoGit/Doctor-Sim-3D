using UnityEngine;

[System.Serializable]
public class AdsRemoteSettings : ScriptableObject
{
    #region Instance + SaveNLoad

    const string prefsKey = "AdsSettings_Key";
    static AdsRemoteSettings m_Instance;
    public static AdsRemoteSettings Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<AdsRemoteSettings>("RemoteSettings");
                m_Instance.Load();
            }

            return m_Instance;
        }
    }

    public void Save(string json)
    {
        Debug.Log("Saved RemoteSettings: \n\n" + json);
        PlayerPrefs.SetString(prefsKey, json);
        PlayerPrefs.Save();
        Load();
    }

    public void Load()
    {
        try
        {
            if (PlayerPrefs.HasKey(prefsKey))
            {
                string json = PlayerPrefs.GetString(prefsKey);
                JsonUtility.FromJsonOverwrite(json, m_Instance);
            }
            else
            {
                PlayerPrefs.SetString(prefsKey, JsonUtility.ToJson(Resources.Load<AdsRemoteSettings>("RemoteSettings")));
                PlayerPrefs.Save();
            }
        }
        catch (System.Exception e)
        {
            PlayerPrefs.SetString(prefsKey, JsonUtility.ToJson(Resources.Load<AdsRemoteSettings>("RemoteSettings")));
            PlayerPrefs.Save();
        }

        Debug.Log("Loaded RemoteSettings: \n\n" + PlayerPrefs.GetString(prefsKey));
    }

    public static void Initialize()
    {
        if (m_Instance == null)
        {
            m_Instance = Resources.Load<AdsRemoteSettings>("RemoteSettings");
            m_Instance.Load();
        }
    }

    #endregion

    public bool ShowInternetPopup = true;
    public bool ShowMediationOn2GB = false;
    public int NextInterstitialDelay = 10;
}