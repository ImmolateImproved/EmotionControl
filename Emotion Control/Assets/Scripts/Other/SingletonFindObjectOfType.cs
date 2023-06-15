using UnityEngine;

public class SingletonFindObjectOfType<T> : MonoBehaviour
    where T: MonoBehaviour
{
    private static T singleton;

    public static T Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<T>();

            return singleton;
        }
    }
}
