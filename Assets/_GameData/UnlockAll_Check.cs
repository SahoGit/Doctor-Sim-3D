using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockAll_Check : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(OnCheckGetUnlockAllPopup);
        if (PlayerPrefs.GetInt("UnlockAll") == 1)
        {
            gameObject.transform.parent.GetComponent<Button>().enabled = true;
            Destroy(gameObject);
        }else
        {
            gameObject.transform.parent.GetComponent<Button>().enabled = false;
        }
    }


    public void OnCheckGetUnlockAllPopup()
    {
//        Sound_Controller.instance.OnCheckPlayStatickClick(0);
       // Popup_Handler.instance.Unlock_AllpopUp.SetActive(true);
    }
}
