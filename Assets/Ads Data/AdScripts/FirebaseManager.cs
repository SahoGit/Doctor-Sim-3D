using Firebase;
using Firebase.Analytics;
using System.Collections;
using UnityEngine;

public static class FirebaseManager
{
    public static bool hasInitialized;
    public static void Initialize()
    {
        hasInitialized = false;
        InitFirebase();
    }

    static void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Enabling firebase Analytics");
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                hasInitialized = true;
            }
        });
    }
}