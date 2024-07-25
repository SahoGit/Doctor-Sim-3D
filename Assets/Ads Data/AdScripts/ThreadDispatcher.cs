using GameAnalyticsSDK.Events;
using GameAnalyticsSDK;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> adEventsQueue = new Queue<Action>();
    private static volatile bool adEventsQueueEmpty = true;

    public static void Enqueue(Action action)
    {
        lock (adEventsQueue)
        {
            adEventsQueue.Enqueue(action);
            adEventsQueueEmpty = false;
        }
    }

    void LateUpdate()
    {
        if (adEventsQueueEmpty) return;

        lock (adEventsQueue)
        {
            while (adEventsQueue.Count > 0)
            {
                try
                {
                    adEventsQueue.Dequeue()?.Invoke();
                }
                catch (Exception e)
                {
                    if (AdConstants.IsDebugBuild)
                        Debug.LogException(e);
                    else
                    {
                        //AppMetrica.Instance.ReportError(e, "Dispatcher");
                        GA_Debug.HandleLog($"Dispatcher : {e.Message}", e.StackTrace, LogType.Exception);
                    }
                }

                adEventsQueueEmpty = true;
            }
        }
    }

    private void OnDisable()
    {
        isInitialized = false;
    }

    #region Instance

    static bool isInitialized;
    public static void Initialize()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            GameObject go = new GameObject("Thread Dispatcher");
            go.AddComponent<ThreadDispatcher>();
            go.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(go);
        }
    }

    #endregion
}
