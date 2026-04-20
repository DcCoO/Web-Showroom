using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null) _instance = (T)FindFirstObjectByType(typeof(T));            
            return _instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (_instance == null) _instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
}