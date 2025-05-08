using UnityEngine;
using UnityEngine.EventSystems;

public class SingletonEventSystem : MonoBehaviour
{
    private static SingletonEventSystem instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (GetComponent<StandaloneInputModule>() == null)
        {
            gameObject.AddComponent<StandaloneInputModule>();
        }
    }
}
