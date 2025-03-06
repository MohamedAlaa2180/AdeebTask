using UnityEngine;

public class GenericSingletonClass<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    string name = typeof(T).Name;
                    Debug.LogWarning($"No {name} found, so creating one.");
                    GameObject obj = new GameObject(name);
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (transform.parent != null)
            {
                DontDestroyOnLoad(transform.parent);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}