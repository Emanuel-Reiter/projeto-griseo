using UnityEngine;

public class Logger : Singleton<Logger>
{   
    public static void Write(string msg)
    {
        Debug.Log($"[Log] {msg}");
    }

    public static void Waring(string msg)
    {
        Debug.Log($"<color=yellow>[Warning]</color> {msg}");
    }

    public static void Error(string msg)
    {
        Debug.Log($"<color=red>[Error]</color> {msg}");
    }

    public static void Started(string msg)
    {
        Debug.Log($"<color=orange>[Started]</color> {msg}");
    }

    public static void Finalized(string msg)
    {
        Debug.Log($"<color=green>[Finalized]</color> {msg}");
    }

    public static void Processed(string msg)
    {
        Debug.Log($"<color=cyan>[Processed]</color> {msg}");
    }

    public static void Success(string msg)
    {
        Debug.Log($"<color=green>[Success]</colord> {msg}");
    }
}
