using UnityEngine;
using UnityEngine.UI;

public class SendAnalyticsDetails : MonoBehaviour
{
    public EventType eventType = EventType.RewardedAd;
    public string SceneName = "MainMenu";

    [DrawIf("eventType", EventType.RewardedAd)]
    public string ReasonForCallingAd = "Get Coins";

    [DrawIf("eventType", EventType.IAP)]
    public bool IsRandomPop = false;

    [DrawIf("eventType", EventType.IAP)]
    public bool FromLoadingGif= false;

    [DrawIf("eventType", EventType.IAP)]
    public string CallingPlace = "Shop";


    private Button CurrentButton;
    private void Start()
    {
        CurrentButton = gameObject.GetComponent<Button>();
        CurrentButton.onClick.AddListener(ReportClickedEvent);
        
    }
    public void ReportClickedEvent()
    {
        //Debug.Log("Event fired ");
        if(eventType== EventType.RewardedAd)
        {
            //AssignAdIds_CB.instance.SendAnalytics("RewardedAd", SceneName, ReasonForCallingAd);
        }
        else if (eventType == EventType.IAP)
        {
            string temp = CallingPlace;
            if (IsRandomPop)
            {
                temp = CallingPlace + " through Random Pop Up";
            }
            else if (FromLoadingGif)
            {
                temp = CallingPlace + " From Loading Gif";
            }

            //AssignAdIds_CB.instance.SendAnalytics("IAP", SceneName, temp);
        }
    }
   
    public enum EventType
    {
        RewardedAd = 0,
        IAP = 1
    }
}
