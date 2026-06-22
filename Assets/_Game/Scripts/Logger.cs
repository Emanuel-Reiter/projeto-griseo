using UnityEngine;

public class Logger : Singleton<Logger>
{   
    public static void Write(string msg)
    {
        Debug.Log($"[Log] {msg}");
    }

    public static void Waring(string msg)
    {
        Debug.Log($"[Warning] {msg}");
    }

    public static void Error(string msg)
    {
        Debug.Log($"[Error] {msg}");
    }

    public static void Started(string msg)
    {
        Debug.Log($"[Started] {msg}");
    }

    public static void Finilized(string msg)
    {
        Debug.Log($"[Finilized] {msg}");
    }

    public static void Processed(string msg)
    {
        Debug.Log($"[Processed] {msg}");
    }

    public static void Success(string msg)
    {
        Debug.Log($"[Success] {msg}");
    }
}
