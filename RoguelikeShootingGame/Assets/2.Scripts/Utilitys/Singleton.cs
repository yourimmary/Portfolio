using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _uniqueInstance;

    public static T Instance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = FindAnyObjectByType<T>();
                if (_uniqueInstance == null)
                {
                    GameObject obj = new GameObject(typeof(T).ToString() + "Obj");
                    _uniqueInstance = obj.AddComponent<T>();
                }
                DontDestroyOnLoad(_uniqueInstance.gameObject);
            }
            return _uniqueInstance;
        }
    }

    protected abstract void SetInit();

    private void OnEnable()
    {
        SetInit();
    }
}
