using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyEditorScriptForCanvasRaycast : MonoBehaviour
{
    //public Image[] AllImages;
    //public Text[] AllText;

    public List<Transform> allTransforms;
    public bool RaycastTarget = false;
    public bool IsMaskable = false;
    private void OnValidate()
    {
        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        allTransforms.Clear();
        Transform[] NewObjs;
        for (int i = 0; i < allObjects.Length; i++)
        {
            NewObjs = allObjects[i].transform.GetComponentsInChildren<Transform>(true);
            for (int j = 0; j < NewObjs.Length; j++)
            {
                allTransforms.Add(NewObjs[j]);
            }
            Array.Clear(NewObjs, 0, NewObjs.Length);
        }
        Array.Clear(allObjects, 0, allObjects.Length);

        for (int i = 0; i < allTransforms.Count; i++)
        {
            if (allTransforms[i].GetComponent<Image>() != null)
            {
                if (allTransforms[i].GetComponent<Button>() == null)
                    allTransforms[i].GetComponent<Image>().raycastTarget = RaycastTarget;

                allTransforms[i].GetComponent<Image>().maskable = IsMaskable;

            }
            if (allTransforms[i].GetComponent<Text>() != null)
            {
                allTransforms[i].GetComponent<Text>().raycastTarget = RaycastTarget;
                allTransforms[i].GetComponent<Text>().maskable = IsMaskable;
            }
        }

    }
}
