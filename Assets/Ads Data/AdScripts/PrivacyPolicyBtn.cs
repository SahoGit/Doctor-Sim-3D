using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PrivacyPolicyBtn : MonoBehaviour
{
    public void VisitWebsite()
    {
        AdsManager.Instance.VisitPrivacyPolicy();
    }
}
