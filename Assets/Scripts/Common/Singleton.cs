using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance {
        get {
            if ( _applicationIsQuitting ) {
                Debug.LogWarning( $"[Singleton] Instance of {typeof( T )} is already destroyed due to application quitting." );
                return null;
            }

            lock ( _lock ) {
                if ( _instance == null ) {
                    _instance = FindFirstObjectByType<T>();

                    if ( _instance == null ) {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof( T )} (Singleton)";
                        DontDestroyOnLoad( singletonObject );
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake() {
        if ( _instance == null ) {
            _instance = this as T;
            DontDestroyOnLoad( gameObject );
        }
        else if ( _instance != this ) {
            Destroy( gameObject );
        }
    }

    protected virtual void OnApplicationQuit() {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy() {
        if ( _instance == this ) {
            _applicationIsQuitting = true;
        }
    }
}
