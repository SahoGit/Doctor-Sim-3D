using UnityEngine;

[DisallowMultipleComponent]
public class AppOpenAdapter : MonoBehaviour
{
    private void Start()
    {
        if (AdConstants.ShowAppOpenAfterwards)
            AdsManager.Instance.SetAppOpenAutoShow(true);
    }

    private void OnDisable()
    {
        if (AdsManager.Instance != null)
            AdsManager.Instance.SetAppOpenAutoShow(false);
    }
}
