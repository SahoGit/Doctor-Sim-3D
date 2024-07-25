using System;
using UnityEngine;

public static class MonetizationLogger
{
    public static void Log(string message)
    {
        Debug.Log("[Monetization] " + message);
    }
    public static void LogWarning(string message)
    {
        Debug.LogWarning("[Monetization] " + message);

    }
    public static void LogError(string message)
    {
        Debug.LogError("[Monetization] " + message);

    }
    public static void LogException(string message, Exception e)
    {

        LogError("[Monetization] " + message);
        LogError(e.StackTrace);

    }
}