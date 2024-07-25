using UnityEngine;
using UnityEngine.Events;

public class RewardedAdAdapter : MonoBehaviour
{
    public string PlacementName;
    public UnityEvent OnRewardComplete;

    public void CallRewardedAd()
    {
        AdsManager.Instance.ShowRewarded(()=>
        {
            OnRewardComplete?.Invoke();

        }, PlacementName);
    }
}
