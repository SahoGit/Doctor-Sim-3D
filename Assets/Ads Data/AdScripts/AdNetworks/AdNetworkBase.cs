using UnityEngine;

public abstract class AdNetworkBase : MonoBehaviour
{
    protected bool isInitialized;
    public abstract void Initialize();
    public abstract bool HasInterstitial(bool doRequest);
    public abstract bool HasRewarded(bool doRequest);
}