using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T I { get; private set; }

    protected virtual void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(this.gameObject);
            return;
        }

        I = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}
