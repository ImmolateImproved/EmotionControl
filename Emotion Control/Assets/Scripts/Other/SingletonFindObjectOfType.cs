using UnityEngine;

public class SingletonFindObjectOfType<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Singleton { get; private set; }

    public virtual void Awake()
    {
        Singleton = this as T;
    }
}
